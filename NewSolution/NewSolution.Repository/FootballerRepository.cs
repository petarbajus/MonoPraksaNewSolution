using NewSolution.Model;
using NewSolution.Repository.Common;
using Npgsql;

namespace NewSolution.Repository
{
    public class FootballerRepository : IFootballerRepository
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=FootballClub";
        public FootballerRepository()
        {
            
        }

        public bool DeleteFootballerById(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "DELETE FROM \"Footballer\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var numberOfCommits = command.ExecuteNonQuery();
                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
        }

        public Footballer GetFootballerById(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Footballer\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Footballer
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        DOB = reader.IsDBNull(2) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(2)),
                        ClubId = reader.GetGuid(3)
                    };
                }
                return null;
            }
            catch (NpgsqlException)
            {
                return null;
            }
        }

        public List<Footballer> GetFootballers()
        {
            var footballers = new List<Footballer>();

            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Footballer\";";
                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    footballers.Add(new Footballer
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        DOB = reader.IsDBNull(2) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(2)),
                        ClubId = reader.GetGuid(3)
                    });
                }
            }
            catch (NpgsqlException)
            {
                return null;
            }

            return footballers;
        }

        public bool InsertFootballer(Footballer footballer)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "INSERT INTO \"Footballer\" (\"Id\", \"Name\", \"DOB\", \"ClubId\") VALUES (@id, @name, @dob, @clubId);";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@name", footballer.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@dob", footballer.DOB.HasValue ? footballer.DOB.Value : DBNull.Value);
                command.Parameters.AddWithValue("@clubId", footballer.ClubId != Guid.Empty ? footballer.ClubId : DBNull.Value);


                connection.Open();
                var numberOfCommits = command.ExecuteNonQuery();
                connection.Close();

                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
             
                return false;
            }
        }

        /*
         try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                string commandText = "INSERT INTO \"Club\" (\"Id\", \"Name\", \"CharacteristicColor\", \"FoundationDate\") VALUES (@id, @name, @characteristicColor, @foundationDate);";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@name", club.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@characteristicColor", club.CharacteristicColor ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@foundationDate", club.FoundationDate.HasValue ? (object)club.FoundationDate.Value : DBNull.Value);

                connection.Open();
                var numberOfCommits = command.ExecuteNonQuery();
                connection.Close();

                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
         */


        public bool UpdateFootballerById(Guid id, Footballer footballer)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                // Step 1: Retrieve the current footballer details
                var getCommandText = "SELECT \"Id\", \"Name\", \"DOB\", \"ClubId\" FROM \"Footballer\" WHERE \"Id\" = @id;";
                using var getCommand = new NpgsqlCommand(getCommandText, connection);
                getCommand.Parameters.AddWithValue("@id", id);

                using var reader = getCommand.ExecuteReader();
                if (!reader.HasRows)
                {
                    return false; // Footballer not found
                }

                // Read the current footballer details from the database
                Footballer currentFootballer = new Footballer();
                reader.Read();
                currentFootballer.Id = Guid.Parse(reader["Id"].ToString());
                currentFootballer.Name = reader["Name"].ToString();
                currentFootballer.DOB = reader["DOB"] != DBNull.Value ? DateOnly.FromDateTime(DateTime.Parse(reader["DOB"].ToString())) : (DateOnly?)null;
                currentFootballer.ClubId = string.IsNullOrEmpty(reader["ClubId"].ToString()) ? null : Guid.Parse(reader["ClubId"].ToString());


                connection.Close();

                // Step 2: Determine which fields need to be updated
                var updateStatements = new List<string>();

                if (!string.IsNullOrEmpty(footballer.Name) && footballer.Name != currentFootballer.Name)
                {
                    updateStatements.Add("\"Name\" = @name");
                }

                if (footballer.DOB.HasValue && footballer.DOB.Value != currentFootballer.DOB)
                {
                    updateStatements.Add("\"DOB\" = @dob");
                }

                if (footballer.ClubId.HasValue && footballer.ClubId.Value != currentFootballer.ClubId)
                {
                    updateStatements.Add("\"ClubId\" = @clubId");
                }

                // If no fields are to be updated, return
                if (updateStatements.Count == 0)
                {
                    return true;
                }

                // Step 3: Build the final SQL command
                var updateCommandText = $"UPDATE \"Footballer\" SET {string.Join(", ", updateStatements)} WHERE \"Id\" = @id;";

                using var updateCommand = new NpgsqlCommand(updateCommandText, connection);

                // Add necessary parameters
                updateCommand.Parameters.AddWithValue("@id", id);

                if (updateStatements.Contains("\"Name\" = @name"))
                {
                    updateCommand.Parameters.AddWithValue("@name", footballer.Name);
                }

                if (updateStatements.Contains("\"DOB\" = @dob"))
                {
                    updateCommand.Parameters.AddWithValue("@dob", footballer.DOB.Value.ToDateTime(new TimeOnly()));
                }

                if (updateStatements.Contains("\"ClubId\" = @clubId"))
                {
                    updateCommand.Parameters.AddWithValue("@clubId", footballer.ClubId.Value);
                }

                connection.Open();
                var rowsAffected = updateCommand.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
        }

    }
}

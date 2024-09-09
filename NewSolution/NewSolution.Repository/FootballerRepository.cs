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

        public async Task<bool> DeleteFootballerByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "DELETE FROM \"Footballer\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var numberOfCommits = await command.ExecuteNonQueryAsync();
                connection.Close();
                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
        }

        public async Task<Footballer> GetFootballerByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Footballer\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
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

        public async Task<List<Footballer>> GetFootballersAsync()
        {
            var footballers = new List<Footballer>();

            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Footballer\";";
                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var footballer = new Footballer();
                    footballer.Id = Guid.Parse(reader["Id"].ToString());
                    footballer.Name = reader["Name"].ToString();
                    footballer.DOB = reader["DOB"] != DBNull.Value ? DateOnly.FromDateTime(DateTime.Parse(reader["DOB"].ToString())) : (DateOnly?)null;
                    footballer.ClubId = string.IsNullOrEmpty(reader["ClubId"].ToString()) ? null : Guid.Parse(reader["ClubId"].ToString());
                    footballers.Add(footballer);
                }
                connection.Close();
            }
            catch (NpgsqlException)
            {
                return null;
            }

            return footballers;
        }

        public async Task<bool> InsertFootballerAsync(Footballer footballer)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "INSERT INTO \"Footballer\" (\"Id\", \"Name\", \"DOB\", \"ClubId\") VALUES (@id, @name, @dob, @clubId);";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, Guid.NewGuid());
                command.Parameters.AddWithValue("@name", footballer.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@dob", footballer.DOB.HasValue ? footballer.DOB.Value : DBNull.Value);
                var clubIdParam = new NpgsqlParameter("@clubId", NpgsqlTypes.NpgsqlDbType.Uuid);
                if (footballer.ClubId == Guid.Empty || footballer.ClubId == null)
                {
                    clubIdParam.Value = DBNull.Value;
                }
                else
                {
                    clubIdParam.Value = footballer.ClubId;
                }
                command.Parameters.Add(clubIdParam);



                connection.Open();
                var numberOfCommits = await command.ExecuteNonQueryAsync();
                connection.Close();

                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
             
                return false;
            }
        }

       


        public async Task<bool> UpdateFootballerByIdAsync(Guid id, Footballer footballer)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

               
                var getCommandText = "SELECT \"Id\", \"Name\", \"DOB\", \"ClubId\" FROM \"Footballer\" WHERE \"Id\" = @id;";
                using var getCommand = new NpgsqlCommand(getCommandText, connection);
                getCommand.Parameters.AddWithValue("@id", id);

                using var reader = await getCommand.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    return false; // Footballer not found
                }

                
                Footballer currentFootballer = new Footballer();
                await reader.ReadAsync();
                currentFootballer.Id = Guid.Parse(reader["Id"].ToString());
                currentFootballer.Name = reader["Name"].ToString();
                currentFootballer.DOB = reader["DOB"] != DBNull.Value ? DateOnly.FromDateTime(DateTime.Parse(reader["DOB"].ToString())) : (DateOnly?)null;
                currentFootballer.ClubId = string.IsNullOrEmpty(reader["ClubId"].ToString()) ? null : Guid.Parse(reader["ClubId"].ToString());


                connection.Close();

                
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

                // If no fields are to be updated, return true
                if (updateStatements.Count == 0)
                {
                    return true;
                }

               
                var updateCommandText = $"UPDATE \"Footballer\" SET {string.Join(", ", updateStatements)} WHERE \"Id\" = @id;";

                using var updateCommand = new NpgsqlCommand(updateCommandText, connection);

              
                updateCommand.Parameters.AddWithValue("@id", id);

                if (updateStatements.Contains("\"Name\" = @name"))
                {
                    updateCommand.Parameters.AddWithValue("@name", footballer.Name);
                }

                if (updateStatements.Contains("\"DOB\" = @dob"))
                {
                    updateCommand.Parameters.AddWithValue("@dob", footballer.DOB);
                }

                if (updateStatements.Contains("\"ClubId\" = @clubId"))
                {
                    updateCommand.Parameters.AddWithValue("@clubId", footballer.ClubId);
                }

                connection.Open();
                var rowsAffected = await updateCommand.ExecuteNonQueryAsync();
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

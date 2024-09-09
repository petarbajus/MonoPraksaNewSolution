using NewSolution.Model;
using NewSolution.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewSolution.Repository
{
    public class ClubRepository : IClubRepository
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=FootballClub";

        public bool DeleteClubById(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "DELETE FROM \"Club\" WHERE \"Id\" = @id;";
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

        public Club GetClubById(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Club\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Club
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        FoundationDate = reader.IsDBNull(2) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(2)),
                        CharacteristicColor = reader.GetString(3)
                    };
                }
                return null;
            }
            catch (NpgsqlException)
            {
                return null;
            }
        }

        public List<Club> GetClubs()
        {
            var clubs = new List<Club>();

            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Club\";";
                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clubs.Add(new Club
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        FoundationDate = reader.IsDBNull(2) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(2)),
                        CharacteristicColor = reader.GetString(3)
                    });
                }
            }
            catch (NpgsqlException)
            {
                return null;
            }

            return clubs;
        }

        public bool InsertClub(Club club)
        {
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
        }

        public bool UpdateClubById(Guid id, Club club)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                // Step 1: Retrieve the current club details
                var getCommandText = "SELECT \"Id\", \"Name\", \"CharacteristicColor\", \"FoundationDate\" FROM \"Club\" WHERE \"Id\" = @id;";
                using var getCommand = new NpgsqlCommand(getCommandText, connection);
                getCommand.Parameters.AddWithValue("@id", id);

                using var reader = getCommand.ExecuteReader();
                if (!reader.HasRows)
                {
                    return false; // Club not found
                }

                // Read the current club details from the database
                reader.Read();
                var currentClub = new Club
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    CharacteristicColor = reader.IsDBNull(reader.GetOrdinal("CharacteristicColor"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("CharacteristicColor")),
                    FoundationDate = reader.IsDBNull(reader.GetOrdinal("FoundationDate"))
                        ? (DateOnly?)null
                        : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("FoundationDate")))
                };

                // Step 2: Determine which fields need to be updated
                var updateStatements = new List<string>();
                if (!string.IsNullOrEmpty(club.Name) && club.Name != currentClub.Name)
                {
                    updateStatements.Add("\"Name\" = @name");
                }

                if (!string.IsNullOrEmpty(club.CharacteristicColor) && club.CharacteristicColor != currentClub.CharacteristicColor)
                {
                    updateStatements.Add("\"CharacteristicColor\" = @characteristicColor");
                }

                if (club.FoundationDate.HasValue && club.FoundationDate.Value != currentClub.FoundationDate)
                {
                    updateStatements.Add("\"FoundationDate\" = @foundationDate");
                }

                // If no fields are to be updated, return
                if (updateStatements.Count == 0)
                {
                    return true;
                }

                // Step 3: Build and execute the update command
                var updateCommandText = $"UPDATE \"Club\" SET {string.Join(", ", updateStatements)} WHERE \"Id\" = @id;";
                using var updateCommand = new NpgsqlCommand(updateCommandText, connection);

                // Add necessary parameters
                updateCommand.Parameters.AddWithValue("@id", id);

                if (updateStatements.Contains("\"Name\" = @name"))
                {
                    updateCommand.Parameters.AddWithValue("@name", club.Name);
                }

                if (updateStatements.Contains("\"CharacteristicColor\" = @characteristicColor"))
                {
                    updateCommand.Parameters.AddWithValue("@characteristicColor", club.CharacteristicColor);
                }

                if (updateStatements.Contains("\"FoundationDate\" = @foundationDate"))
                {
                    updateCommand.Parameters.AddWithValue("@foundationDate", club.FoundationDate.Value.ToDateTime(new TimeOnly()));
                }

                var rowsAffected = updateCommand.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
        }



    }
}

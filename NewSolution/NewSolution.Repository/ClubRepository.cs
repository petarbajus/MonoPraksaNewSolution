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

        public async Task<bool> DeleteClubByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "DELETE FROM \"Club\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var numberOfCommits = await command.ExecuteNonQueryAsync();
                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
        }

        public async Task<Club> GetClubByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Club\" WHERE \"Id\" = @id;";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
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

        public async Task<List<Club>> GetClubsAsync()
        {
            var clubs = new List<Club>();

            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Club\";";
                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var club = new Club();
                    club.Id = Guid.Parse(reader["Id"].ToString());
                    club.Name = reader["Name"].ToString();
                    club.FoundationDate = reader["FoundationDate"] != DBNull.Value ? DateOnly.FromDateTime(DateTime.Parse(reader["FoundationDate"].ToString())) : (DateOnly?)null;
                    clubs.Add(club);
                }
            }
            catch (NpgsqlException)
            {
                return null;
            }

            return clubs;
        }

        public async Task<bool> InsertClubAsync(Club club)
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
                var numberOfCommits = await command.ExecuteNonQueryAsync();
                connection.Close();

                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
                return false;
            }
        }

       
        public async Task<bool> UpdateClubByIdAsync(Guid id, Club club)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                
                var getCommandText = "SELECT \"Id\", \"Name\", \"CharacteristicColor\", \"FoundationDate\" FROM \"Club\" WHERE \"Id\" = @id;";
                using var getCommand = new NpgsqlCommand(getCommandText, connection);
                getCommand.Parameters.AddWithValue("@id", id);

                using var reader = await getCommand.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    return false; // Club not found
                }

               
                await reader.ReadAsync();
                var currentClub = new Club();

                currentClub.Id = Guid.Parse(reader["Id"].ToString());
                currentClub.Name = reader["Name"].ToString();
                currentClub.FoundationDate = reader["FoundationDate"] != DBNull.Value ? DateOnly.FromDateTime(DateTime.Parse(reader["FoundationDate"].ToString())) : (DateOnly?)null;
                currentClub.CharacteristicColor = string.IsNullOrEmpty(reader["CharacteristicColor"].ToString()) ? null : reader["CharacteristicColor"].ToString();

                connection.Close();

                
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

                
                if (updateStatements.Count == 0)
                {
                    return true;
                }

                
                var updateCommandText = $"UPDATE \"Club\" SET {string.Join(", ", updateStatements)} WHERE \"Id\" = @id;";
                using var updateCommand = new NpgsqlCommand(updateCommandText, connection);

                
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
                    updateCommand.Parameters.AddWithValue("@foundationDate", club.FoundationDate);
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

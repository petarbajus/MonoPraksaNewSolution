using NewSolution.Model;
using NewSolution.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

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
                var commandText = @"
                 SELECT c.""Id"" AS ClubId, c.""Name"" AS ClubName, c.""FoundationDate"", c.""CharacteristicColor"",
                 f.""Id"" AS FootballerId, f.""Name"" AS FootballerName, f.""DOB""
                 FROM ""Club"" c
                 LEFT JOIN ""Footballer"" f ON f.""ClubId"" = c.""Id""
                 WHERE c.""Id"" = @id;";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                Club club = null;

                while (await reader.ReadAsync())
                {
                    // Only instantiate the club once, since we might get multiple rows for each footballer
                    if (club == null)
                    {
                        club = new Club
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            FoundationDate = !reader.IsDBNull(reader.GetOrdinal("FoundationDate")) ? DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("FoundationDate"))) : (DateOnly?)null,
                            CharacteristicColor = reader.IsDBNull(reader.GetOrdinal("CharacteristicColor")) ? null : reader.GetString(reader.GetOrdinal("CharacteristicColor")),
                            Footballers = new List<Footballer>()
                        };
                    }

                    // Check if there is a footballer in the current row
                    if (!reader.IsDBNull(reader.GetOrdinal("FootballerId")))
                    {
                        var footballer = new Footballer
                        {
                            Id = reader.GetGuid(4),
                            Name = reader.GetString(5),
                            DOB = !reader.IsDBNull(6) ? (DateOnly?)DateOnly.FromDateTime(reader.GetDateTime(6)) : null,
                            ClubId = id
                        };

                        club.Footballers.Add(footballer);
                    }
                }
                connection.Close();

                return club;
            }
            catch (NpgsqlException)
            {
                // Log the exception
                return null;
            }
        }


        public async Task<List<Club>> GetClubsAsync()
        {
            var clubs = new Dictionary<Guid, Club>();

            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = "SELECT * FROM \"Club\" c\r\nINNER JOIN \"Footballer\" fb ON fb.\"ClubId\" = c.\"Id\";";
                using var command = new NpgsqlCommand(commandText, connection);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var clubId = reader.GetGuid(reader.GetOrdinal("Id"));
                    if (!clubs.TryGetValue(clubId, out var club))
                    {
                        club = new Club
                        {
                            Id = clubId,
                            Name = reader.GetString(1),
                            FoundationDate = !reader.IsDBNull(reader.GetOrdinal("FoundationDate")) ? DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("FoundationDate"))) : (DateOnly?)null,
                            CharacteristicColor = reader.IsDBNull(reader.GetOrdinal("CharacteristicColor")) ? null : reader.GetString(reader.GetOrdinal("CharacteristicColor")),
                            Footballers = new List<Footballer>()
                        };
                        clubs.Add(clubId, club);
                    }

                    var footballer = new Footballer
                    {
                        Id = reader.GetGuid(4),
                        Name = reader.GetString(5),
                        DOB = !reader.IsDBNull(6) ? (DateOnly?)DateOnly.FromDateTime(reader.GetDateTime(6)) : null,
                        ClubId = clubId
                    };
                    club.Footballers.Add(footballer);
                }
                connection.Close();
                
            }
            catch (NpgsqlException ex)
            {
                // Log the error or handle it as required
                return null;
            }
            

            return clubs.Values.ToList();
        }


        public async Task<bool> InsertClubAsync(Club club)
        {
            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                string commandText = "INSERT INTO \"Club\" (\"Id\", \"Name\", \"CharacteristicColor\", \"FoundationDate\") VALUES (@id, @name, @characteristicColor, @foundationDate);";

                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", club.Id);
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

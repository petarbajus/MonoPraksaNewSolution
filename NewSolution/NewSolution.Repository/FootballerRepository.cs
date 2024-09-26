using NewSolution.Common;
using NewSolution.Model;
using NewSolution.Repository.Common;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NewSolution.Repository
{
    public class FootballerRepository : IFootballerRepository
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=FootballClub";

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

                // SQL query to join Footballer and Club tables
                var commandText = @"
                SELECT f.""Id"" AS FootballerId, f.""Name"" AS FootballerName, f.""DOB"", f.""ClubId"",
                 c.""Id"" AS ClubId, c.""Name"" AS ClubName, c.""FoundationDate"", c.""CharacteristicColor""
                FROM ""Footballer"" f
                LEFT JOIN ""Club"" c ON f.""ClubId"" = c.""Id""
                WHERE f.""Id"" = @id;";


                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    // Map footballer and club data
                    var footballer = new Footballer
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("FootballerId")),
                        Name = reader.GetString(reader.GetOrdinal("FootballerName")),
                        DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DOB"))),
                        ClubId = reader.IsDBNull(reader.GetOrdinal("ClubId")) ? null : reader.GetGuid(reader.GetOrdinal("ClubId")),
                        Club = new Club
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("ClubId")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("ClubId")),
                            Name = reader.IsDBNull(reader.GetOrdinal("ClubName")) ? null : reader.GetString(reader.GetOrdinal("ClubName")),
                            FoundationDate = reader.IsDBNull(reader.GetOrdinal("FoundationDate")) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("FoundationDate"))),
                            CharacteristicColor = reader.IsDBNull(reader.GetOrdinal("CharacteristicColor")) ? null : reader.GetString(reader.GetOrdinal("CharacteristicColor"))
                        }
                    };
                    return footballer;
                }

                return null;
            }
            catch (NpgsqlException ex)
            {

                return null;
            }

        }
            public async Task<List<Footballer>> GetFootballersAsync(FootballerFilter footballerFilter, Paging paging, Sorting sorting)
            {
            var footballers = new List<Footballer>();

            try
            {
                using var connection = new NpgsqlConnection(ConnectionString);
                var commandText = new StringBuilder();

                commandText.Append("SELECT f.\"Id\" AS FootballerId, ");
                commandText.Append("f.\"Name\" AS FootballerName, ");
                commandText.Append("f.\"DOB\", ");
                commandText.Append("f.\"ClubId\", ");
                commandText.Append("c.\"Name\" AS ClubName, ");
                commandText.Append("c.\"FoundationDate\", ");
                commandText.Append("c.\"CharacteristicColor\" ");   
                commandText.Append("FROM \"Footballer\" f ");
                commandText.Append("LEFT JOIN \"Club\" c ON f.\"ClubId\" = c.\"Id\" ");




                // Start the WHERE clause for filtering
                commandText.Append("WHERE 1=1 ");

                // Apply filters (based on FootballerFilter if provided)
                if (!string.IsNullOrEmpty(footballerFilter.SearchQuery))
                {
                    commandText.Append("AND f.\"Name\" ILIKE @SearchQuery ");
                }

                if (footballerFilter.DOBFrom.HasValue)
                {
                    commandText.Append("AND f.\"DOB\" >= @DateOfBirthFrom ");
                }

                if (footballerFilter.DOBTo.HasValue)
                {
                    commandText.Append("AND f.\"DOB\" <= @DateOfBirthTo ");
                }

                if (footballerFilter.ClubId.HasValue)
                {
                    commandText.Append("AND f.\"ClubId\" = @ClubId ");
                }

                if (!string.IsNullOrEmpty(sorting.SortBy))
                {
                    commandText.Append("ORDER BY ");
                    commandText.Append($"{sorting.SortBy}");

                    if (!string.IsNullOrEmpty(sorting.SortDirection) &&
                        (string.Equals(sorting.SortDirection, "asc", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(sorting.SortDirection, "desc", StringComparison.OrdinalIgnoreCase)))
                    {
                        commandText.Append($" {sorting.SortDirection.ToUpper()} ");
                    }
                    else
                    {
                        commandText.Append(" ASC ");  // Default to ASC if no valid SortDirection is provided
                    }
                }

                if (paging.RecordsPerPage > 0)
                {
                    commandText.Append("LIMIT @RecordsPerPage OFFSET @Offset ");
                }

                using var command = new NpgsqlCommand(commandText.ToString(), connection);

                if (!string.IsNullOrEmpty(footballerFilter.SearchQuery))
                {
                    command.Parameters.AddWithValue("@SearchQuery", $"%{footballerFilter.SearchQuery}%");
                }

                if (footballerFilter.DOBFrom.HasValue)
                {
                    command.Parameters.AddWithValue("@DateOfBirthFrom", footballerFilter.DOBFrom.Value);
                }

                if (footballerFilter.DOBTo.HasValue)
                {
                    command.Parameters.AddWithValue("@DateOfBirthTo", footballerFilter.DOBTo.Value);
                }

                if (footballerFilter.ClubId.HasValue)
                {
                    command.Parameters.AddWithValue("@ClubId", footballerFilter.ClubId.Value);
                }

                if (paging.RecordsPerPage > 0)
                {
                    command.Parameters.AddWithValue("@RecordsPerPage", paging.RecordsPerPage);
                    command.Parameters.AddWithValue("@Offset", (paging.CurrentPage - 1) * paging.RecordsPerPage);
                }

                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var footballer = new Footballer();

                    footballer.Id = reader.GetGuid(0);

                    footballer.Name = reader.IsDBNull(1) ? null : reader.GetString(1);

                    if (!reader.IsDBNull(2))  // Index 2 should be the column index for DOB
                    {
                        footballer.DOB = DateOnly.FromDateTime(reader.GetDateTime(2));
                    }
                    else
                    {
                        footballer.DOB = null;
                    }

                    footballer.Club = new Club();

                    if (!reader.IsDBNull(3))
                    {
                        footballer.Club.Id = reader.GetGuid(3);

                        footballer.Club.Name = reader.GetString(4);
                    }

                    if (!reader.IsDBNull(5)) 
                    {
                        footballer.Club.FoundationDate = DateOnly.FromDateTime(reader.GetDateTime(5));
                    }
                    else
                    {
                        footballer.Club.FoundationDate = null;
                    }

                    footballer.Club.CharacteristicColor = reader.IsDBNull(6) ? null : reader.GetString(6); 

                    footballers.Add(footballer);
                }
                connection.Close();
            }
            catch (NpgsqlException)
            {
                // Log the exception or handle it as necessary
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
                command.Parameters.AddWithValue("@id", footballer.Id);
                command.Parameters.AddWithValue("@name", footballer.Name ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@dob", footballer.DOB.HasValue ? footballer.DOB.Value : DBNull.Value);

                if (footballer.ClubId == null || footballer.ClubId == Guid.Empty)
                {
                    command.Parameters.AddWithValue("@clubId", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@clubId", footballer.ClubId);
                }

                connection.Open();
                var numberOfCommits = await command.ExecuteNonQueryAsync();  // Execute the insert command

                //If a ClubId is specified, fetch the club details after the footballer has been inserted
                /*footballer.Club = new Club();

                if (footballer.ClubId != null && footballer.ClubId != Guid.Empty)
                {
                    var getClubCommandText = "SELECT \"Id\", \"Name\", \"FoundationDate\", \"CharacteristicColor\" FROM \"Club\" WHERE \"Id\" = @clubId;";
                    using var getCommand = new NpgsqlCommand(getClubCommandText, connection);
                    getCommand.Parameters.AddWithValue("@clubId", footballer.ClubId);

                    using var reader = await getCommand.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        footballer.Club = new Club
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            FoundationDate = reader.IsDBNull(reader.GetOrdinal("FoundationDate")) ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("FoundationDate"))),
                            CharacteristicColor = reader.IsDBNull(reader.GetOrdinal("CharacteristicColor")) ? null : reader.GetString(reader.GetOrdinal("CharacteristicColor"))
                        };
                    }
                }*/

                connection.Close();
                return numberOfCommits > 0;
            }
            catch (NpgsqlException)
            {
                // Handle the exception accordingly
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

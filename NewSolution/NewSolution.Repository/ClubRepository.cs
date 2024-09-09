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
                var commandText = "INSERT INTO \"Club\" (\"Id\", \"Name\", \"CharacteristicColor\", \"FoundationDate\") VALUES (@id, @name, @characteristicColor, @foundationDate);";
                using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", Guid.NewGuid());
                command.Parameters.AddWithValue("@name", club.Name);
                command.Parameters.AddWithValue("@characteristicColor", club.CharacteristicColor);
                command.Parameters.AddWithValue("@foundationDate", club.FoundationDate.HasValue ? (object)club.FoundationDate.Value.ToDateTime(new TimeOnly()) : DBNull.Value);

                connection.Open();
                var numberOfCommits = command.ExecuteNonQuery();
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

                var getCommandText = "SELECT * FROM \"Club\" WHERE \"Id\" = @id;";
                using var getCommand = new NpgsqlCommand(getCommandText, connection);
                getCommand.Parameters.AddWithValue("@id", id);

                connection.Open();
                using var reader = getCommand.ExecuteReader();
                if (!reader.Read())
                {
                    return false;
                }

                List<string> updateStatements = new List<string>();
                var parameters = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@id", id)
                };

                if (!string.IsNullOrEmpty(club.Name) && !reader.GetString(1).Equals(club.Name, StringComparison.Ordinal))
                {
                    updateStatements.Add("\"Name\" = @name");
                    parameters.Add(new NpgsqlParameter("@name", club.Name));
                }

                if (!string.IsNullOrEmpty(club.CharacteristicColor) && !reader.GetString(3).Equals(club.CharacteristicColor, StringComparison.Ordinal))
                {
                    updateStatements.Add("\"CharacteristicColor\" = @characteristicColor");
                    parameters.Add(new NpgsqlParameter("@characteristicColor", club.CharacteristicColor));
                }

                if (club.FoundationDate.HasValue && reader.IsDBNull(2) || reader.GetDateTime(2) != club.FoundationDate.Value.ToDateTime(new TimeOnly()))
                {
                    updateStatements.Add("\"FoundationDate\" = @foundationDate");
                    parameters.Add(new NpgsqlParameter("@foundationDate", club.FoundationDate.Value.ToDateTime(new TimeOnly())));
                }

                if (updateStatements.Count == 0)
                {
                    return true;
                }

                var updateCommandText = $"UPDATE \"Club\" SET {string.Join(", ", updateStatements)} WHERE \"Id\" = @id;";
                using var updateCommand = new NpgsqlCommand(updateCommandText, connection);
                updateCommand.Parameters.AddRange(parameters.ToArray());

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

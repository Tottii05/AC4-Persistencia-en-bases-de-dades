using ac3.Business.DTOs;
using Npgsql;

namespace ac3
{
    public class ContactDAO : IContactDAO
    {
        private readonly string connectionString;

        public ContactDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public ConsumDTO GetConsumById(int id)
        {
            ConsumDTO consum = null;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM consum WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            consum = new ConsumDTO
                            {
                                Any = reader.GetInt32(1),
                                Codi_Comarca = reader.GetInt32(3),
                                Comarca = reader.GetString(2),
                                Poblacio = reader.GetInt32(4),
                                Domestic_xarxa = reader.GetInt32(5),
                                Activitats_economiques_i_fonts_propies = reader.GetInt32(6),
                                Total = reader.GetInt32(7),
                                Consum_domestic_per_capita = reader.GetDouble(8)
                            };
                        }
                    }
                }
            }
            return consum;
        }
        public List<ConsumDTO> GetAllConsum()
        {
            List<ConsumDTO> consums = new List<ConsumDTO>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM consum", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            consums.Add(GetConsumById(reader.GetInt32(0)));
                        }
                    }
                }
            }
            return consums;
        }
        public void AddConsum(ConsumDTO consum)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO consum (year, comarca, codi_comarca, poblacio, domestic_xarxa, activitats_economiques_i_fonts_propies, total, consum_domestic_per_capita) VALUES (@any, @comarca, @codi_comarca, @poblacio, @domestic_xarxa, @activitats_economiques_i_fonts_propies, @total, @consum_domestic_per_capita)", connection))
                {
                    command.Parameters.AddWithValue("any", consum.Any);
                    command.Parameters.AddWithValue("comarca", consum.Comarca);
                    command.Parameters.AddWithValue("codi_comarca", consum.Codi_Comarca);
                    command.Parameters.AddWithValue("poblacio", consum.Poblacio);
                    command.Parameters.AddWithValue("domestic_xarxa", consum.Domestic_xarxa);
                    command.Parameters.AddWithValue("activitats_economiques_i_fonts_propies", consum.Activitats_economiques_i_fonts_propies);
                    command.Parameters.AddWithValue("total", consum.Total);
                    command.Parameters.AddWithValue("consum_domestic_per_capita", consum.Consum_domestic_per_capita);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateConsum(ConsumDTO consum)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE consum SET any = @any, codi_comarca = @codi_comarca, comarca = @comarca, poblacio = @poblacio, domestic_xarxa = @domestic_xarxa, activitats_economiques_i_fonts_propies = @activitats_economiques_i_fonts_propies, total = @total, consum_domestic_per_capita = @consum_domestic_per_capita WHERE comarca = @comarca", connection))
                {
                    command.Parameters.AddWithValue("any", consum.Any);
                    command.Parameters.AddWithValue("codi_comarca", consum.Codi_Comarca);
                    command.Parameters.AddWithValue("comarca", consum.Comarca);
                    command.Parameters.AddWithValue("poblacio", consum.Poblacio);
                    command.Parameters.AddWithValue("domestic_xarxa", consum.Domestic_xarxa);
                    command.Parameters.AddWithValue("activitats_economiques_i_fonts_propies", consum.Activitats_economiques_i_fonts_propies);
                    command.Parameters.AddWithValue("total", consum.Total);
                    command.Parameters.AddWithValue("consum_domestic_per_capita", consum.Consum_domestic_per_capita);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteConsum(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("DELETE FROM consum WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
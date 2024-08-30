using System.Data;
using Npgsql;
using NpgsqlTypes;

Console.WriteLine("Hello, World!");

var connString = "Host=<host>;Username=<username>;Password=<password>;Database=<database>";

await using var conn = new NpgsqlConnection(connString);
await conn.OpenAsync();

Console.WriteLine("Connected to PostgreSQL");               

await using (var transaction = await conn.BeginTransactionAsync())
{
    try
    {
        await using (var command = new NpgsqlCommand("sample_procedure", conn))
        {
            command.CommandType = CommandType.StoredProcedure;

            /*
                OUT errcd double precision,
                OUT errmsg text,
                IN in_inv_sys text,
                IN in_lifnr text,
                IN in_cc text,
                OUT c_inv refcursor
            */

            command.Parameters.Add(new NpgsqlParameter("errcd", NpgsqlDbType.Integer) 
            { 
                Direction = ParameterDirection.Output 
            });

            command.Parameters.Add(new NpgsqlParameter("errmsg", NpgsqlDbType.Text) 
            { 
                Direction = ParameterDirection.Output 
            });
            
            command.Parameters.Add(new NpgsqlParameter("in_inv_sys", NpgsqlDbType.Text) 
            { 
                Direction = ParameterDirection.Input, Value = "SAP" 
            });
            
            command.Parameters.Add(new NpgsqlParameter("in_lifnr", NpgsqlDbType.Text) 
            { 
                Direction = ParameterDirection.Input, 
                Value = "1000000000" 
            });

            command.Parameters.Add(new NpgsqlParameter("in_cc", NpgsqlDbType.Text) 
            { 
                Direction = ParameterDirection.Input, Value = "US" 
            });

            command.Parameters.Add(new NpgsqlParameter("c_inv", NpgsqlDbType.Refcursor) 
            { 
                Direction = ParameterDirection.InputOutput,
                Value = "c_inv"
            });

            command.ExecuteNonQuery();

            Console.WriteLine("Executed stored procedure");
            
            // Fetch the refcursor

            command.CommandText = "fetch all in \"c_inv\"";
            command.CommandType = CommandType.Text;

            await using var reader = await command.ExecuteReaderAsync();

            Console.WriteLine("Fetched refcursor");

            int ordinal = reader.GetOrdinal("company_c");

            while (await reader.ReadAsync())
                Console.WriteLine(reader.GetString(ordinal));
        }

        await transaction.CommitAsync();

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        await transaction.RollbackAsync();
    }
}






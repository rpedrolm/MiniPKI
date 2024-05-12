using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using Microsoft.EntityFrameworkCore;

using System.Data;
using System.Data.SqlClient;

// Microsoft.SqlServer.Server



/*
dotnet add package MySql.Data
dotnet add package Microsoft.Data.SqlClient

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer 8.0.1

// instala: Microsoft.EntityFrameworkCore y Microsoft.EntityFrameworkCore.SqlServer

// Modo correcto de crear tablas auto increment al hacer insert omitir Id y listo
//
CREATE TABLE DemoOkAutoInc (
   Id       INT IDENTITY(1,1) PRIMARY KEY,
   IdFacu   INT NOT NULL,
   Nombres  VARCHAR(50),
   Obs      VARCHAR(50)
)
*/


namespace MiniPki.Models;

//{
/*
public class Usuarios
{
    public int Id { get; set; }
    public int Estado { get; set; }
    public int IdTipo { get; set; }
    public int IdCA { get; set; }
    public string? Empresa { get; set; }
    public string? Responsable { get; set; }
    public string? Usuario { get; set; }
    public string? Clave { get; set; }
    public string? FechaReg { get; set; }
}

public class Archivos
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int Padre { get; set; }
    public int Tipo { get; set; }
    public string? Nombre { get; set; }
    public string? Obs { get; set; }
    public string? Fecha { get; set; }
}

//---------------------------------------------------------------------------------------
// Conexion a BD1 y sus N tablas
//---------------------------------------------------------------------------------------
public class MyData : DbContext
{
    // tablas conectadas a la definicion struct class
    //
    public virtual DbSet<Usuarios> Usuarios { get; set; }
    public virtual DbSet<Archivos> Archivos { get; set; }



    public MyData(DbContextOptions<MyData> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {           
        var strconn = @"Data Source=MiniPki.mssql.somee.com;Initial Catalog=MiniPki;User Id=minipki_SQLLogin_1;Password=huqgll1nqx;TrustServerCertificate=True;";

        // options.UseSqlServer(strconn);
        optionsBuilder.UseSqlServer(strconn, options =>
        {
            options.UseRelationalNulls();
            options.EnableRetryOnFailure();
        });
    }
*/

/*
public IEnumerable<string> ListaTablas()
{
    using (var command = Database.GetDbConnection().CreateCommand())
    {
        command.CommandText = "SELECT * FROM information_schema.TABLES ORDER BY TABLE_TYPE, TABLE_NAME";
        Database.OpenConnection();

        using (var result = command.ExecuteReader())
        {
            var tablas = new List<string>();

            while (result.Read())
            {
                //tablas.Add( "[" + result.GetString(3) +"] >> "+result.GetString(2) );
                tablas.Add( result.GetString(2) );
            }

            return tablas;
        }
    }
}

public IEnumerable<string> ListaColumnas( string tabla )
{
    using (var command = Database.GetDbConnection().CreateCommand())
    {
        if( tabla.Length > 0 )
            command.CommandText = "SELECT * FROM information_schema.COLUMNS WHERE TABLE_NAME = '" +tabla+ "'";
        else
            command.CommandText = "SELECT * FROM information_schema.COLUMNS";

        Database.OpenConnection();

        using (var result = command.ExecuteReader())
        {
            var colums = new List<string>();

            while (result.Read())
            {
                colums.Add( result.GetString(2)+"."+result.GetString(3) + " ("+ result.GetString(7) + ")" );
            }

            return colums;
        }
    }
}

public IEnumerable<List<string>> VerTabla( string tabla )
{
    if( tabla == null )
        return null;


    using (var command = Database.GetDbConnection().CreateCommand())
    {
        // Agregar datos
        //data.Add(new List<string> { "1", "John", "Doe" });
        //data.Add(new List<string> { "2", "Jane", "Smith" });



        command.CommandText = "SELECT * FROM " + tabla;

        Database.OpenConnection();

        using (var reader = command.ExecuteReader())
        {
            var data = new List< List<string> >();

            // Mostrar nombres de columnas
            var colums = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                colums.Add( reader.GetName(i) );

                //Console.Write($"{reader.GetName(i)}\t");
            }

            data.Add( colums );
            //Console.WriteLine();

            // Mostrar datos
            while ( reader.Read() )
            {
                var colum = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    colum.Add( reader[i].ToString() );
                    //Console.Write($"{reader[i]}\t");
                }
                //Console.WriteLine();
                data.Add( colum );
            }

            return data;
        }
    }
}*/
/*
}

//---------------------------------------------------------------------------------------
// Conexion a BD2 y sus N tablas
//---------------------------------------------------------------------------------------
public class DbSecondo : DbContext
{
public DbSecondo(DbContextOptions<DbSecondo> options) : base(options)
{
}
}
}
*/
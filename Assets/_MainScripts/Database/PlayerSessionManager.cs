using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSessionManager : MonoBehaviour
{
    private string connectionString;

    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/player_sessions.db";
        Debug.Log("ConnectionString: " + connectionString);
    }

     // Função para recuperar informações de sessões de jogadores do banco de dados
    public List<PlayerSession> RetrievePlayerSessions()
    {
        List<PlayerSession> playerSessions = new List<PlayerSession>();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string query = "SELECT * FROM PlayerSessions";
                dbCmd.CommandText = query;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PlayerSession session = new PlayerSession();
                        session.PlayerID = reader["PlayerID"].ToString();
                        session.GameTitle = reader["GameTitle"].ToString();
                        session.EntryTime = DateTime.Parse(reader["EntryTime"].ToString());
                        session.ExitTime = DateTime.Parse(reader["ExitTime"].ToString());
                        playerSessions.Add(session);
                    }
                }
            }

            dbConnection.Close();
        }

        return playerSessions;
    }

    // Inserir um novo jogo
    public void InsertGame(string gameTitle, string gameImage)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Consulta SQL para verificar se o jogo já existe na tabela Games
                string checkQuery = "SELECT ID FROM Games WHERE GameTitle = @GameTitle";
                dbCmd.CommandText = checkQuery;

                dbCmd.Parameters.Add(new SqliteParameter("@GameTitle", gameTitle));

                object result = dbCmd.ExecuteScalar();

                if (result != null)
                {
                    // O jogo já existe, não insira novamente
                    Debug.LogWarning("O jogo com título '" + gameTitle + "' já existe na tabela Games.");
                }
                else
                {
                    // O jogo não existe, insira um novo
                    string insertQuery = "INSERT INTO Games (GameTitle, GameImage) VALUES (@GameTitle, @GameImage)";
                    dbCmd.CommandText = insertQuery;

                    // Adicione os parâmetros
                    dbCmd.Parameters.Add(new SqliteParameter("@GameTitle", gameTitle));
                    dbCmd.Parameters.Add(new SqliteParameter("@GameImage", gameImage));

                    // Execute a instrução de inserção
                    dbCmd.ExecuteNonQuery();
                }
            }

            dbConnection.Close();
        }
    }

    // Inserir uma nova sessão de jogador

    public void InsertPlayerSession(string playerID, string gameTitle, DateTime entryTime, DateTime exitTime)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            // Verifique se o jogo já existe na tabela Games
            int gameId = GetGame(gameTitle);

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Consulta SQL para inserir dados em PlayerSessions
                string insertQuery = "INSERT INTO PlayerSessions (PlayerID, GameID, EntryTime, ExitTime) VALUES (@PlayerID, @GameID, @EntryTime, @ExitTime)";
                dbCmd.CommandText = insertQuery;

                // Adicione os parâmetros
                dbCmd.Parameters.Add(new SqliteParameter("@PlayerID", playerID));
                dbCmd.Parameters.Add(new SqliteParameter("@GameID", gameId));
                dbCmd.Parameters.Add(new SqliteParameter("@EntryTime", entryTime.ToString("yyyy-MM-dd HH:mm:ss")));
                dbCmd.Parameters.Add(new SqliteParameter("@ExitTime", exitTime.ToString("yyyy-MM-dd HH:mm:ss")));

                // Execute a instrução de inserção
                dbCmd.ExecuteNonQuery();
            }

            dbConnection.Close();
        }
    }

    // Consulta de tempo de uso diário
    public List<DailyUsageRecord> RetrieveDailyUsage()
    {
        List<DailyUsageRecord> dailyUsageRecords = new List<DailyUsageRecord>();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Consulta SQL para recuperar o tempo de uso por dia
                string query = "SELECT Date, GameTitle, SUM(UsageTime) AS TotalUsage FROM DailyUsage GROUP BY Date, GameTitle";
                dbCmd.CommandText = query;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DailyUsageRecord record = new DailyUsageRecord();
                        record.Date = Convert.ToDateTime(reader["Date"]);
                        record.GameTitle = reader["GameTitle"].ToString();
                        record.UsageTime = Convert.ToInt32(reader["TotalUsage"]);
                        dailyUsageRecords.Add(record);
                    }
                }
            }

            dbConnection.Close();
        }

        return dailyUsageRecords;
    }

    // Atualização de tempo de uso diário
    public void UpdateDailyUsage(string gameTitle, int usageTime)
    {
        // Verifique se o jogo já existe na tabela Games
        int gameId = GetGame(gameTitle);

        DateTime currentDate = DateTime.Now.Date;
        string dateString = currentDate.ToString("yyyy-MM-dd");

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            // Criação da tabela DailyUsage, se ela não existir
            string createDailyUsageTableQuery = @"
            CREATE TABLE IF NOT EXISTS DailyUsage (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Date DATETIME,
                GameID INTEGER,
                UsageTime INTEGER
            )";

            using (IDbCommand createTableCmd = dbConnection.CreateCommand())
            {
                createTableCmd.CommandText = createDailyUsageTableQuery;
                createTableCmd.ExecuteNonQuery();
            }

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Consulta SQL para verificar se já existe um registro para este dia, jogo e jogador
                string checkQuery = "SELECT * FROM DailyUsage WHERE Date = @Date AND GameID = @GameID";
                dbCmd.CommandText = checkQuery;

                dbCmd.Parameters.Add(new SqliteParameter("@Date", dateString));
                dbCmd.Parameters.Add(new SqliteParameter("@GameID", gameId));

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Se o registro já existe, atualize o tempo de uso
                        int currentUsageTime = Convert.ToInt32(reader["UsageTime"]);
                        int newUsageTime = currentUsageTime + usageTime;

                        // Consulta SQL para atualizar o tempo de uso
                        string updateQuery = "UPDATE DailyUsage SET UsageTime = @UsageTime WHERE Date = @Date AND GameID = @GameID";
                        dbCmd.CommandText = updateQuery;

                        dbCmd.Parameters.Add(new SqliteParameter("@UsageTime", newUsageTime));

                        dbCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Se o registro não existe, insira um novo
                        string insertQuery = "INSERT INTO DailyUsage (Date, GameID, UsageTime) VALUES (@Date, @GameID, @UsageTime)";
                        dbCmd.CommandText = insertQuery;

                        dbCmd.Parameters.Add(new SqliteParameter("@Date", dateString));
                        dbCmd.Parameters.Add(new SqliteParameter("@GameID", gameId));
                        dbCmd.Parameters.Add(new SqliteParameter("@UsageTime", usageTime));

                        dbCmd.ExecuteNonQuery();
                    }
                }
            }

            dbConnection.Close();
        }
    }

    // Função para verificar se o jogo já existe na tabela Games e retornar o ID do jogo
    private int GetGame(string gameTitle)
    {
    using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                // Consulta SQL para verificar se o jogo já existe na tabela Games
                string checkQuery = "SELECT ID FROM Games WHERE GameTitle = @GameTitle";
                dbCmd.CommandText = checkQuery;

                dbCmd.Parameters.Add(new SqliteParameter("@GameTitle", gameTitle));

                object result = dbCmd.ExecuteScalar();

                if (result != null)
                {
                    // O jogo já existe, retorne o ID
                    return Convert.ToInt32(result);
                }
                else
                {
                    // O jogo não existe, mostre uma mensagem no console e retorne -1
                    Debug.LogWarning("O jogo com título '" + gameTitle + "' não existe na tabela Games.");
                    return -1; // Ou outro valor que indique que o jogo não existe
                }
            }
        }
    }
}

// Classe para representar informações de sessões de jogadores
public class PlayerSession
{
    public string PlayerID { get; set; }
    public string GameTitle { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }
}
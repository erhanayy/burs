using System;
using System.Data.SqlClient;

public class SMSNotification
{
    public int NotificationId { get; set; }
    public DateTime NotificationDate { get; set; }
    public string NotificationType { get; set; }
    public string SMSSender { get; set; }
    public int SMSSendContactId { get; set; }
    public string SMSSubject { get; set; }
    public string SMSBody { get; set; }
    private string connectionString; // Veritabanı bağlantı dizesi

    public SMSNotification(int notificationId, DateTime notificationDate, string notificationType, string smsSender, int smsSendContactId, string smsSubject, string smsBody, string connectionString)
    {
        NotificationId = notificationId;
        NotificationDate = notificationDate;
        NotificationType = notificationType;
        SMSSender = smsSender;
        SMSSendContactId = smsSendContactId;
        SMSSubject = smsSubject;
        SMSBody = smsBody;
        this.connectionString = connectionString;
    }

    public bool InsertIntoDatabase()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Notification (NotificationId, NotificationDate, NotificationType, SMSSender, SMSSendContactId, SMSSubject, SMSBody) VALUES (@NotificationId, @NotificationDate, @NotificationType, @SMSSender, @SMSSendContactId, @SMSSubject, @SMSBody)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NotificationId", this.NotificationId);
                    command.Parameters.AddWithValue("@NotificationDate", this.NotificationDate);
                    command.Parameters.AddWithValue("@NotificationType", this.NotificationType);
                    command.Parameters.AddWithValue("@SMSSender", this.SMSSender);
                    command.Parameters.AddWithValue("@SMSSendContactId", this.SMSSendContactId);
                    command.Parameters.AddWithValue("@SMSSubject", this.SMSSubject);
                    command.Parameters.AddWithValue("@SMSBody", this.SMSBody);

                    command.ExecuteNonQuery();
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Bir hata oluştu: " + ex.Message);
            return false;
        }
    }
}
[HttpGet("/user")]
public IActionResult GetUserProfile()
{
    string userId = Request.Query["id"];
    
    using (var connection = new SqliteConnection("Data Source=users.db"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        
        // Oh boy... SQL Injection via string interpolation
        command.CommandText = $"SELECT * FROM users WHERE user_id = {userId}";
        
        using (var reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                // What are 'temp' and 'res'?
                var temp = ProcessData(reader); 
                var res = FormatResponse(temp);
                return Ok(res);
            }
        }
    }
    return NotFound();
}

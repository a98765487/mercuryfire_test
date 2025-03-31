using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class MyOfficeACPDController : ControllerBase
{
    private readonly IConfiguration _config;
    public MyOfficeACPDController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] object jsonData)
    {
        string json = jsonData.ToString();
        Guid groupId = Guid.NewGuid();
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("usp_MyOffice_ACPD_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@json", json);
                cmd.ExecuteNonQuery();
            }

            using (SqlCommand logCmd = new SqlCommand("usp_AddLog", conn))
            {
                logCmd.CommandType = CommandType.StoredProcedure;

                logCmd.Parameters.AddWithValue("@_InBox_ReadID", 0);
                logCmd.Parameters.AddWithValue("@_InBox_SPNAME", "usp_MyOffice_ACPD_Insert");
                logCmd.Parameters.AddWithValue("@_InBox_GroupID", groupId);
                logCmd.Parameters.AddWithValue("@_InBox_ExProgram", "Insert");
                logCmd.Parameters.AddWithValue("@_InBox_ActionJSON", json);

                var outputParam = new SqlParameter("@_OutBox_ReturnValues", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };
                logCmd.Parameters.Add(outputParam);

                logCmd.ExecuteNonQuery();

                string logResult = outputParam.Value.ToString();
                return Ok(new {  message = logResult });
            }
        }
    }
}

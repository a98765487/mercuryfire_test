using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class MyOfficeACPDController : ControllerBase
{
    private readonly IConfiguration _config;
    public MyOfficeACPDController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost]
    public IActionResult Create([FromBody] object jsonData)
    {
        return ExecuteSp("usp_MyOffice_ACPD_Insert", "Insert", jsonData);
    }

    [HttpPut]
    public IActionResult Update([FromBody] object jsonData)
    {
        return ExecuteSp("usp_MyOffice_ACPD_Update", "Update", jsonData);
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] object jsonData)
    {
        return ExecuteSp("usp_MyOffice_ACPD_Delete", "Delete", jsonData);
    }

    [HttpGet]
    public IActionResult Read()
    {
        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("usp_MyOffice_ACPD_Read", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string json = reader.GetString(0);
                        return Ok(json);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }
    }

    private IActionResult ExecuteSp(string spName, string actionName, object jsonData)
    {
        string json = jsonData.ToString();
        Guid groupId = Guid.NewGuid();

        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@json", json);
                cmd.ExecuteNonQuery();
            }

            using (SqlCommand logCmd = new SqlCommand("usp_AddLog", conn))
            {
                logCmd.CommandType = CommandType.StoredProcedure;

                logCmd.Parameters.AddWithValue("@_InBox_ReadID", 0);
                logCmd.Parameters.AddWithValue("@_InBox_SPNAME", spName);
                logCmd.Parameters.AddWithValue("@_InBox_GroupID", groupId);
                logCmd.Parameters.AddWithValue("@_InBox_ExProgram", actionName);
                logCmd.Parameters.AddWithValue("@_InBox_ActionJSON", json);

                var outputParam = new SqlParameter("@_OutBox_ReturnValues", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };
                logCmd.Parameters.Add(outputParam);

                logCmd.ExecuteNonQuery();

                string logResult = outputParam.Value.ToString();
                return Ok(new
                {
                    message = $"{actionName} жие\",
                    groupId = groupId,
                    log = logResult
                });
            }
        }
    }
}
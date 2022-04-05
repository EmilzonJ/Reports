using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Reports.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[EnableCors("AllowAllOrigins")]
public class ReportViewerController : ControllerBase, IReportController
{
    private IMemoryCache _cache;

    public ReportViewerController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpPost]
    public object PostReportAction(Dictionary<string, object> jsonResult)
    {
        return ReportHelper.ProcessReport(jsonResult, this, _cache);
    }

    public void OnInitReportOptions(ReportViewerOptions reportOption)
    {
        var currentPath = Directory.GetCurrentDirectory();
        var reportName = reportOption.ReportModel.ReportPath.Split("/").Last();
        var reportPath = Path.Combine(currentPath, "wwwroot", "Resources", reportName);
        FileStream reportStream =
            new FileStream(reportPath, FileMode.Open, FileAccess.Read);
        reportOption.ReportModel.Stream = reportStream;
    }

    [NonAction]
    public void OnReportLoaded(ReportViewerOptions reportOption)
    {
    }

    [NonAction]
    public object PostFormReportAction()
    {
        throw new NotImplementedException();
    }

    [ActionName("GetResource")]
    [AcceptVerbs("GET")]
    public object GetResource(ReportResource resource)
    {
        return ReportHelper.GetResource(resource, this, _cache);
    }
}
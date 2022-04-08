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
        var reportPath = reportOption.ReportModel.ReportPath;
        FileStream reportStream =
            new FileStream(reportPath, FileMode.Open, FileAccess.Read);
        reportOption.ReportModel.Stream = reportStream;
    }

    [NonAction]
    public void OnReportLoaded(ReportViewerOptions reportOption)
    {
    }

    [HttpPost]
    public object PostFormReportAction()
    {
        return ReportHelper.ProcessReport(null, this, _cache);
    }

    [ActionName("GetResource")]
    [AcceptVerbs("GET")]
    public object GetResource(ReportResource resource)
    {
        return ReportHelper.GetResource(resource, this, _cache);
    }
}
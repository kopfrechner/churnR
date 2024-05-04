using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class ChartJsReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileChurns)
    {
        commitCount = string.Join(",", fileChurns.OrderByDescending(x => x.CommitCount).Select(x => "{" + $"x:'{x.FileName}',y:{x.CommitCount}" + "}"));
        lineChurns = string.Join(",", fileChurns.OrderByDescending(x => x.TotalLineChurns).Select(x => "{" + $"x:'{x.FileName}',y:{x.TotalLineChurns}" + "}"));
        
        Out.Write(HtmlTemplate);
    }

    private static string commitCount = "";
    private static string lineChurns = "";
    
    private string HtmlTemplate => @"
<!DOCTYPE html>
<html lang=""en"">
   <body>
      <div>
         <canvas id=""ChurnR""></canvas>
      </div>
      <script src=""https://cdn.jsdelivr.net/npm/chart.js""></script>
      <script>
         const ctx = document.getElementById('ChurnR');
         const data = {
         datasets: 
         [{
             label: 'Involved in Commits',
             data: ["+commitCount+@"],
             fill: false,
             borderColor: 'rgb(75, 192, 192)',
             tension: 0.1
           },
           {
             label: 'Total Lines Added and Deleted',
             data: ["+lineChurns+@"],
             fill: false,
             borderColor: 'rgb(192, 75, 192)',
             tension: 0.1
           }
         ]};
         
         new Chart(ctx, {
           type: 'line',
           data: data
         });
      </script>
   </body>
</html>
";

}


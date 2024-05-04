using System.Globalization;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class ChartJsReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileChurns)
    {
        var churns = fileChurns
            .Select(file => new
            {
                UniqueFileName = fileChurns.Count(x => x.FileName.Equals(file.FileName)) == 1
                    ? file.FileName
                    : file.FullFileName,
                file.CommitCount,
                file.TotalLineChurns,
                file.AverageLineChurnsPerCommit,
                FileNameCount = file.HistoricFullFileNames.Count
            }).ToList();
        
        commitCount = string.Join(",", churns.OrderByDescending(x => x.CommitCount).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.CommitCount}" + "}"));
        lineChurns = string.Join(",", churns.OrderByDescending(x => x.TotalLineChurns).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.TotalLineChurns}" + "}"));
        averageChurnsPerCommit = string.Join(",", churns.OrderByDescending(x => x.AverageLineChurnsPerCommit).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.AverageLineChurnsPerCommit.ToString(CultureInfo.InvariantCulture)}" + "}"));
        fileNameCount = string.Join(",", churns.OrderByDescending(x => x.FileNameCount).Select(x => "{" + $"x:'{x.UniqueFileName}',y:{x.FileNameCount}" + "}"));
        
        Out.Write(HtmlTemplate);
    }

    private static string commitCount = "";
    private static string lineChurns = "";
    private static string averageChurnsPerCommit = "";
    private static string fileNameCount = "";
    
    private string HtmlTemplate => @"
        <!DOCTYPE html>
        <html lang=""en"">
           <body>
              <div>
                 <canvas id=""commitCount""></canvas>
              </div>
              <div>
                 <canvas id=""lineChurns""></canvas>
              </div>
              <div>
                 <canvas id=""averageChurnsPerCommit""></canvas>
              </div>
              <div>
                 <canvas id=""fileNameCount""></canvas>
              </div>
              <script src=""https://cdn.jsdelivr.net/npm/chart.js""></script>
              <script>
                 new Chart(document.getElementById('commitCount'), {
                   type: 'line',
                   data: {
                     datasets: 
                     [{
                         label: 'Involved in Commits',
                         data: ["+commitCount+@"],
                         fill: false,
                         borderColor: 'rgb(75, 192, 192)',
                         tension: 0.1
                       }
                     ]}
                 });

                 new Chart(document.getElementById('lineChurns'), {
                   type: 'line',
                   data: {
                     datasets: 
                     [{
                         label: 'Total lines added and deleted',
                         data: ["+lineChurns+@"],
                         fill: false,
                         borderColor: 'rgb(192, 75, 192)',
                         tension: 0.1
                       }
                     ]}
                 });

                 new Chart(document.getElementById('averageChurnsPerCommit'), {
                   type: 'line',
                   data: {
                     datasets: 
                     [{
                         label: 'Average Churns / Commit',
                         data: ["+averageChurnsPerCommit+@"],
                         fill: false,
                         borderColor: 'rgb(192, 192, 75)',
                         tension: 0.1
                       }
                     ]}
                 });

                 new Chart(document.getElementById('fileNameCount'), {
                   type: 'line',
                   data: {
                     datasets: 
                     [{
                         label: 'Rename or Move Count',
                         data: ["+fileNameCount+@"],
                         fill: false,
                         borderColor: 'rgb(75, 156, 215)',
                         tension: 0.1
                       }
                     ]}
                 });
              </script>
           </body>
        </html>
        ";
}


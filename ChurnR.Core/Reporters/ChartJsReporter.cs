using ChurnR.Core.Processors;

namespace ChurnR.Core.Reporters;

public class ChartJsReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<KeyValuePair<string, int>> fileChurns)
    {
        labelsString = "\"" + string.Join("\",\"", fileChurns.Select(x => x.Key.Trim('"'))) + "\"";
        churnsString = string.Join(",", fileChurns.Select(x => x.Value));

        Out.Write(HtmlTemplate);
    }

    private static string labelsString = "";
    private static string churnsString = "";
    
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
         
         const labels = ["+labelsString+@"];
         
         const data = {
         labels: labels,
         datasets: [{
           label: 'ChurnR',
           data: ["+churnsString+@"],
           fill: false,
           borderColor: 'rgb(75, 192, 192)',
           tension: 0.1
         }]
         };
         
         new Chart(ctx, {
           type: 'line',
           data: data
         });
      </script>
   </body>
</html>
";

}


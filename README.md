ChurnR
======

ChurnR helps to asses the churn level of your files in your repository.  
Churn can help you detect which files are changed the most in their lifetime. 
This helps identify potential bug hives, and improper design.

Easily install ChurnR as dotnet tool and start analyze your code churn

```
dotnet tool install --global Kopfrechner.ChurnR
```

ChurnR currently supports file-level churns for

* Git
* SVN (experimental)

As outputs, ChurnR supports

* ChartJs
* Simple
* Table
* CSV
* XML

Amongst others, ChurnR can also take top # of items to display, cut off churn level, and a date to go back up to. See "Getting Started".

Background
----------------
From this work http://research.microsoft.com/apps/pubs/default.aspx?id=69126

    Code is not static; it evolves over time to meet new requirements. The way code
    evolved in the past can be used to predict its evolution in the future. In particular,
    there is an often accepted notion that code that changes a lot is of lower quality—
    and thus more defect-prone than unchanged code.

    Key Points
    - The more a component has changed (churned), the more likely it is to have
      defects.
    Code churn measures can be used to predict defect-prone components.

ChurnR currently gives a view of *file churn*. 
However, if churn data is combined with complexity, it gets even more meaningful. 
Cross-check file churn with a static code analysis tools to get complex files with a huge file churn, so-called [Hotspots](https://www.adamtornhill.com/articles/code-quality-in-context/why-i-write-dirty-code.html).

Getting Started
---------------

Initial help 

    $ ChurnR help
    git        Git Repository
    svn        Subversion Repository
    help       Display more information on a specific command.
    version    Display version information.

Help for git repositories

	$ ChurnR git --help
    -d, --from-date    Past date to calculate churn from. Absolute in dd-mm-yyyy or number of days back from now.
    -c, --churn        Minimal churn. Specify either a number for minimum, or float for precent.
    -t, --top          Return this number of top records.
    -r, --report       Type of report to output. Use one of: table (default), xml, csv, chartjs, simple
    -x, --exclude      Exclude resources matching a list of regular expressions
    -n, --include      Include resources matching this regular expression
    -o, --output       When set, writes the report to a specific file, to console otherwise.
    -p, --path         Set path to your repository otherwise defaults to current directory.
    --help             Display this help screen.
    --version          Display version information.

Any combination of parameters work.

	$ ChurnR git -t 5 -c 3                # take top 5, cut off at level 3 and below.
	$ ChurnR git -c 0.3                   # display files that consist 30% of all changes (0.3)
	$ ChurnR git -d 24-12-2023            # calculate for 24th of Dec, 2023 up to now.
	$ ChurnR git -c 2 -r chartjs          # cut off at 2, report output as chartjs.
	$ ChurnR git -x exe$ .*json.*         # exclude resources that end with 'exe' or contain 'json'  
    $ ChurnR git -p ../path/to/repo/      # specify a path to the repo to analyze	
    $ ChurnR git -r chartjs -o chart.html # report output as chartjs and write to chart.html file

This is a sample output for using ChartJs on ChurnR-repository:
    
    $ ChurnR.exe git -r chartjs -o chart.html -n cs$

![Commits per File](https://github.com/kopfrechner/churnR/blob/master/Assets/CommitsPerFile.png)
![Total line churn per File](https://github.com/kopfrechner/churnR/blob/master/Assets/TotalLineChurnPerFile.png)
![Average Churn Per Commit](https://github.com/kopfrechner/churnR/blob/master/Assets/AverageChurnPerCommitPerFile.png)
![File renames or moves](https://github.com/kopfrechner/churnR/blob/master/Assets/RenameOrMovesPerFile.png)

Here is another sample of a run, which cuts off at 8, and uses the default table report:

	$ ChurnR git -c 8 -t 10
	+-------------------------------------+
    | README.md                      | 18 |
    | ChartJsReporter.cs             | 11 |
    | GitAdapter.cs                  | 11 |
    | ChurnR.csproj                  | 9  |
    | Program.cs                     | 9  |
    | Analyzer.cs                    | 8  |
    | FileStatistics.cs              | 8  |
    | ServiceCollectionExtensions.cs | 8  |
    | AdapterDataSource.cs           | 7  |
    | BaseOptions.cs                 | 7  |
    +-------------------------------------+

And here is an example of taking the top 4 records on ChurnR's git repo, output as xml report.

	$ ChurnR git -t 4 -r xml
	<?xml version="1.0" encoding="Codepage - 850"?>
    <NChurnAnalysisResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
      <FileChurns>
        <FileChurn>
          <File>README.md</File>
          <Value>18</Value>
        </FileChurn>
        <FileChurn>
          <File>ChartJsReporter.cs</File>
          <Value>11</Value>
        </FileChurn>
        <FileChurn>
          <File>GitAdapter.cs</File>
          <Value>11</Value>
        </FileChurn>
        <FileChurn>
          <File>ChurnR.csproj</File>
          <Value>9</Value>
        </FileChurn>
      </FileChurns>
    </NChurnAnalysisResult>

Contribute
----------

ChurnR is an open-source project. Therefore, you are free to help improving it.
There are several ways of contributing to ChurnR's development:

* Build apps using ChurnR and spread the word.
* Bug and features using the issue tracker.
* Submit patches fixing bugs and implementing new functionality.
* Create an ChurnR fork and start hacking. Extra points for using GitHubs pull requests and feature branches.

License
-------

This code is free software; you can redistribute it and/or modify it under the
terms of the Apache License. See LICENSE.txt.
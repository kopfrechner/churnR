ChurnR
======

ChurnR is a utility that helps asses the churn level of your files in your repository.  
Churn can help you detect which files are changed the most in their lifetime. This helps identify potential bug hives, and improper design.  

ChurnR currently supports file-level churns for

* Git
* SVN

As outputs, ChurnR supports

* ChartJs
* Simple
* Table
* CSV
* XML

Amongst others, ChurnR can also take top # of items to display, cut off churn level, and a date to go back up to. See "Getting Started".

Easily install as dotnet tool and start analyze your code churn

```
dotnet tool install --global Kopfrechner.ChurnR
```

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

Getting Started
---------------

Initial help 

    $ ChurnR help
    
    git        Git Repository
    
    svn        Subversion Repository
    
    help       Display more information on a specific command.
    
    version    Display version information.

Help for targeting git repositories

	$ ChurnR git --help
    Option 'h' is unknown.
    
    -d, --from-date    Past date to calculate churn from. Absolute in dd-mm-yyyy or number of days back from now.
    
    -c, --churn        Minimal churn. Specify either a number for minimum, or float for precent.
    
    -t, --top          Return this number of top records.
    
    -r, --report       Type of report to output. Use one of: table (default), xml, csv
    
    -x, --exclude      Exclude resources matching this regular expression
    
    -n, --include      Include resources matching this regular expression
    
    -o, --output       Write to a specific file
    
    --help             Display this help screen.
    
    --version          Display version information.




Any combination of parameters work.

	$ ChurnR git -t 5 -c 3        # take top 5, cut off at level 3 and below.
	$ ChurnR git -c 0.3           # display files that consist 30% of all changes (0.3)
	$ ChurnR git -d 24-12-2010    # calculate for 24th of Dec, 2010 up to now.
	$ ChurnR git -c 2 -r xml      # cut off at 2, report output as XML.
	$ ChurnR git -x exe$          # exclude resources that end with 'exe' 

Here is a sample of a run, which cuts off at 8, and uses the default table report:

	$ ChurnR -c 8
	+--------------------------------------------------+
	| lib/rubikon/application/instance_methods.rb | 48 |
	| lib/rubikon/application.rb                  | 30 |
	| test/test.rb                                | 30 |
	| lib/rubikon/command.rb                      | 28 |
	| lib/rubikon/parameter.rb                    | 17 |
	| test/application_tests.rb                   | 14 |
	| Rakefile                                    | 13 |
	| lib/rubikon/application/dsl_methods.rb      | 12 |
	| README.md                                   | 11 |
	| samples/helloworld/hello_world.rb           | 11 |
	| lib/rubikon.rb                              | 10 |
	| lib/rubikon/exceptions.rb                   | 10 |
	| lib/rubikon/flag.rb                         | 10 |
	| lib/rubikon/action.rb                       | 9  |
	| lib/rubikon/application/base.rb             | 9  |
	| lib/rubikon/option.rb                       | 9  |
	| lib/rubikon/progress_bar.rb                 | 9  |
	| samples/helloworld.rb                       | 9  |
	+--------------------------------------------------+

And here is an example of taking the top 4 records on ChurnR's git repo, output as xml report.

	$ ChurnR -t 4 -r xml
	<?xml version="1.0" encoding="utf-8"?>
	<ChurnRAnalysisResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
	  <FileChurns>
	    <FileChurn>
	      <File>README.md</File>
	      <Value>2</Value>
	    </FileChurn>
	    <FileChurn>
	      <File>.gitignore</File>
	      <Value>1</Value>
	    </FileChurn>
	    <FileChurn>
	      <File>AssemblyInfo.cs</File>
	      <Value>1</Value>
	    </FileChurn>
	    <FileChurn>
	      <File>Gemfile</File>
	      <Value>1</Value>
	    </FileChurn>
	  </FileChurns>
	</ChurnRAnalysisResult>

Contribute
----------

ChurnR is an open-source project. Therefore you are free to help improving it.
There are several ways of contributing to ChurnR's development:

* Build apps using ChurnR and spread the word.
* Bug and features using the issue tracker.
* Submit patches fixing bugs and implementing new functionality.
* Create an ChurnR fork and start hacking. Extra points for using GitHubs pull requests and feature branches.

License
-------

This code is free software; you can redistribute it and/or modify it under the
terms of the Apache License. See LICENSE.txt.
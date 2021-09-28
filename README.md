# Number Translator

> #### Test your skills: Number to Words Web Page
>Please develop a web page featuring a web server routine that converts numerical input into words and passes these words as a string output parameter.
>Output example:
>Input:      “123.45”
>Output:    “ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS”


## Design Considerations

The output is for money but the question statement makes no reference to currency. Should this involve multiple formatters for decimal/currency etc?

Decimal stringified output could include a lot more than 2 decimals..

> string output parameter. 
Just return the value of the output.

**1600**
Could be written as both *one thousand six hundred* and *sixteen hundred*. Should the routine have a pre-determined format or allow the client to select this?

.NET 6's minimal API's would be a perfect use case for such a small web application but not currently in release. Razor pages has been used in its place in the interim as the razor syntax can easily be ported between MVC/Blazor if necessary.



Translator..
    Parse an int
    Break up into portions and retrieve text values
    Hundred / thousand/ million etc.
    Should recognise twenty, twelve etc.

    Break into full and decimal parts then calculate?
    Divide by 10 with recursion to get base 10 column or store & use position within int?
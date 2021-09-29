# Numerical Translator

![Picture of application](AppPage.png?raw=true)

## Design Criteria

> #### Test your skills: Number to Words Web Page
>Please develop a web page featuring a web server routine that converts numerical input into words and passes these words as a string output parameter.
>Output example:
>Input:      “123.45”
>Output:    “ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS”

## Design Considerations

The design criteria just requires to to convert from a number into words, however the example given returns it in a basic currency. Ideally this would be clarified before continuing as currency has completely different limitations to 'numerical input'. For this reason I have designed the routine to just return language representation of the decimal itself *(one tenth etc.)*

This routine will only return a **single** representation of a number. This isn't perfectly ideal as people may have a different idea on when/if *and* should be used to connect groups of numbers. This also misses a common representation of *four* digit numbers. **1600** Could be written as both *one thousand six hundred* and *sixteen hundred*. This routine will represent these as the first case.

.NET 6's minimal API's would be a perfect use case for such a small web application however this is not currently in stable release so .NET Core 3.1 MVC has been used.

Currently the application has been restricted to a 15 digit number to keep the application simple and prove a proof of concept. However the internals of the routine could be very easily adjusted to support larger. The class stores and constructs the result of the whole and fractional portions of the number seperately. Expanding this would just involve adding more base10 results to the BaseTenHelperMethods.GetBaseTenName function and a small modification to the constructor to accept 2 doubles *(whole and fractional)* as an input. This would then already allow up to a double.double return.

## Code Flow

This routine has been designed to try and break up a double into it's base10 *thousands* components. When split into thousands, if the first number exists it will *always* semantically be n 'Hundred' and then you can just calculate the hundreds & tens remainder. This group can then be given a token for which 'thousands' block it is which calls the BaseTenHelperMethods.GetBaseTenName() method to get the string name.

![Visually explaining NumberParser](NumberParser.png?raw=true)

This code can then live completely free of the Parser and still return a correct result *(single/multi-threaded, multi-device etc).*
The parser then just calls itself recursively to break the number down into these *thousands* components and compile the string result. This recursion did have an unfortunate by-product of being unable to correctly independtly combine after its second iteration.
**1 100 100** would be expected to return One Million One Hundred Thousand and One Hundred. But my attempts at correctly joining these strings through the parsers would output One Million **and** One Hundred Thousand and One Hundred - which I was not happy with. To get around this I added another parameter to *ParseWholeNumberLessThanThousand()* method so it could return the right response. This probably isn't ideal as now its internals are tied to its output. It would be much better to just fix the caller *ParseWholeNumber* however I am just a little short on time :(

This could has been written as a single static method and just working through the number one unit at a time, modifying the return string as it progresses. However I didn't like that this would then tie the implementation to the output. Alternatively my class could possibly be expanded upon by converting it to an abstract class with an abstract GetOutput() method. This could then be inherited to provide an alternative output for currency?

The BaseTenHelperMethods class could have been just arrays/dictionaries inside the Translator however I felt since their output doesn't rely on any context, they have the option to be reused if they were public methods.

## Testing

Through development the number parser internals were continually tested, however now that these method is private to the class these have been replaced with the public GetOutput() testing. These tests have been left in the codebase and were what I used throughout the development process. If you have the .NET runtime they can just be easily called through **dotnet test** in console within the NumerticalTranslator.Tests directory. The front-end MVC application was just manually tested.

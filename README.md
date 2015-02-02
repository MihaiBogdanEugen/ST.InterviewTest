# STInterviewTest

### Request:

> Implementeaza un Proof of Concept/Demonstratie pentru un servicu WCF care primeste ca parametru fisier txt sau un PDF, extrage continutul si aplica niste transformari / procesari asupra textului extras. Solutia trebuie gandita astfel incat sa fie usor de extins cu procesari noi.

> Pentru extragerea din PDF o sa folosim SDK-ul Adobe Acrobat (nu e nevoie sa il adaugi efectiv).
Stiind ca folosirea lui face serviciul instabil, ce solutii arhitecturale ai astfel incat solutia sa aiba fiabilitatea cat mai mare?

### How to run:

1. Restore Nuget packages (nuget restore STInterviewTest.sln)
2. Build the solution
3. Set the following 3 projects: **ClassicAnalyzerService.SelfHost**,  **RestfulAnalyzerService.SelfHost** and **ConsoleApp** to Start, make sure the **ConsoleApp** starts lasts!
4. Last but not least, run the tests in STInterviewTest.Infrastructure.Tests





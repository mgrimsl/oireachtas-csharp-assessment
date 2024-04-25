# Propylon Code Test - C# Oireachtas API

This project has a C# file `Program.cs` which defines 3 methods to
load and process a couple of the [Houses of the Oireachtas Open Data APIs][1].

Specifically, they use the data obtained from the `legislation` and `members`
api endpoints to answer the questions:

* Which bills were sponsored by a given member ?
* Which bills were last updated within a specified time period ?

When the program is run it will present the user with a CLI where they can
select their data source and filter type and then enter their filter paramters.

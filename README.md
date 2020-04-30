# DataProcessorSolution
This is a Pilot project for merging files into one standard output.

All the standard output columns are set in appconfig.jon so that it can be kept flexible for future variations.
User just needs to enter the chosen standard fields in the file, as a single comma separated string. For example: AccountCode,Type,Currency

All data mappings for the columns needs to be entered as a json array object. For Example-

[{  "ColumnName":"Currency", "ReplaceValue":"CD","NewValue":"CAD"},{"ColumnName":"Currency", "ReplaceValue":"US","NewValue":"USD"},{"ColumnName":"Type", "ReplaceValue":"1","NewValue":"Trading"},{"ColumnName":"Type", "ReplaceValue":"2","NewValue":"RRSP"},{"ColumnName":"Type", "ReplaceValue":"3","NewValue":"RESP"},{"ColumnName":"Type", "ReplaceValue":"4","NewValue":"Fund"}]


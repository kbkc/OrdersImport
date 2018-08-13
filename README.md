# OrdersImport
Program get orders from webshops databases and write them to *.csv


\bin\Debug\app_cfg.xml  - store database access data, email data, last order id and getting orders query
ReadCfg.cs - data classes and reading config data methods
dbProc.cs - reading data from site and put it to *.csv
Program.cs - sites proceeding cycle.

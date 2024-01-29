# Logging configuration samples

Safewhere Identify offers powerful logging capabilities with numerous settings, which can sometimes make it challenging for customers to decide which ones to use. Below are different logging configuration presets that you can import into your Identify instances:

- `Sample 1` folder: Your logs are stored in the **IdentifyAudit database**. They use the **Information** log level, the **Default** log filter rule, logging **all** event log types, and **excluding** user requests (note that SEC logs already cover all the old user requests events). This preset logs the most data, which might quickly consume all your database storage.
- `Sample 2` folder: Your logs are stored in the **IdentifyAudit database**. They use the **Error** log level, the **Silent** log filter rule, logging only the **SEC** event log type, and **excluding** user requests. This preset logs the least amount of data.
- `Sample 3` folder: Your logs are stored in **Text files**. They use the **Information** log level, the **Default** log filter rule, logging **all** event log types, and **excluding** user requests. This preset logs the most data, which might quickly consume all your disk space.
- `Sample 4` folder: Your logs are stored in **Text files**. They use the **Information** log level, the **Default** log filter rule, logging **all** event log types, and **including** user requests.

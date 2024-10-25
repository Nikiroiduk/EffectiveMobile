# EffectiveMobile

Данные лежат в файле: [orders.csv](https://github.com/Nikiroiduk/EffectiveMobile/blob/master/orders.csv)

### **Приоритет аргументов**
Аргументы командной строки имеют наивысший приоритет, затем идут переменные окружения, и только потом файл конфигурации.

Конфиг: [appsettings.json](https://github.com/Nikiroiduk/EffectiveMobile/blob/master/appsettings.json)
```json
{
  "FilePath": "Путь к файлу с данными",
  "LogPath": "Путь для хранения логов",
  "OutPath": "Путь для хранения результата",
  "FilterDistrict": "Название района для фильтрации",
  "DeliveryTimes": {
    "Start": "Дата заказа",
    "End": "Конечная дата диапазона (необязательное поле)"
  }
}
```
*файл конфигурации должен находиться в одной директории с .exe файлом*



### Запуск из консоли с передачей аргументов:
```cmd
CLI.exe --FilePath <путь> --LogPath <путь> --OutPath <путь> --FilterDistrict <название> --DeliveryTimes:Start <yyyy-MM-ddTHH:mm:ss> --DelivertTimes:End <yyyy-MM-ddTHH:mm:ss>
```

### Запуск из консоли с использоавнием переменных окружения

#### CMD
```cmd
set FilePath=<путь>
set LogPath=<путь>
set OutPath=<путь>
set FilterDistrict=<название>
set DeliveryTimes:Start=<yyyy-MM-dd HH:mm:ss>
set DeliveryTimes:End=<yyyy-MM-dd HH:mm:ss>
CLI.exe
```

#### PowerShell
```powershell
$env:FilePath=<путь>
$env:LogPath=<путь>
$env:OutPath=<путь>
$env:FilterDistrict=<название>
$env:DeliveryTimes_Start=<yyyy-MM-dd HH:mm:ss>
$env:DeliveryTimes_End=<yyyy-MM-dd HH:mm:ss>
./CLI.exe
```


# vsix
В папке два проекта
1. vsixsample.sln
2. consoletest consoletest.csproj (не входит в sln)
Размещены в одной папке. Нужно запускать в отдельных экзмплярах студии.

Общение сделано через WCF NET.PIPE . Можно и в http переделать , принципиально разницы нету, так как REST клиента тут нет.

Три функции
 Добавлять зависимости (nuget и просто по ссылке).
• Добавлять .cs файлы с одним заготовленным классом.
• Писать в открытый .cs файл текст в нужную позицию.

размещены в одном вызове HelloWorld

Для тестирования:
Запустить vsixsample.sln на выполнение
В автоматически открытом экземплярее студии выбрать новок консольное приложение(КП)

Теперь перейти в тестовую консоль consoletest.csproj  и запустить.
В (КП) будет добавлен файл tmp1.cs из VSIX пакета, вставлен перевод строки, и добавлен Ref на Microsoft.Build.

Требуется 4.7.2 установленный. Так как используется абсолютный путь в коде.

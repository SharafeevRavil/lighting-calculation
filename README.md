# lighting-calculation
![image](https://user-images.githubusercontent.com/42946134/235922574-0a8ea985-1358-4c48-bd3f-9edd212a28dd.png)

Проекты в решении:
* Illumination - проект с библиотекой
* Tests - проект с тестами

## О проекте
lighting-calculation - библиотека для расчёта освещённости помещения методом Radiosity.

Расчёт происходит путём разделения плоскостей на отдельные полигоны, именуемые патчами. Для определения объёма светопередачи между двумя патчами используются полукубы, каждая грань которых разделена на ячейки. Затем определяется количество ячеек, покрываемых проекцией получающего свет патча на полукуб испускающего. Зная количество ячеек, можно определить формфактор между испускающим и получающим свет патчем, то есть долю всего испусаемого света, которую получит получающий патч. Рассчитав форм факторы между всеми патчами, долю отражаемого ими света и изначальное количество испускаемого светового потока испускающего патча в люменах, можно определить освещённость всех патчей в пространстве.

## Установка .NET SDK и среды разработки
Для исользования рекомендуется использовать Rider или подходящую IDE, поддерживающую работу с C# и .NET.

Необходимо установить .NET SDK 7.

## Использование:
1) Создать список плоскостей Mesh, указав массив списков точек составляющих его патчей по часовой стрелке. Для нескольких плоскостей указать материал, излучающий свет (fluxEmission). При передаче reMeshingConfig указанные патчи дополнительно поделятся на меньшие патчи, удовлетворяющие условиям конфигурации (максимальная площадь патча и/или максимальная длина стороны патча).
2) Созданный список мешей обернуть в одно пространство Space.
3) Расчитать формфакторы патчей с помощью метода Initialize() переменной пространства Space, указав шаблонный полукуб, копия которого будет использоваться для каждого патча. Увеличивая число ячеек полукуба, можно увеличить точность расчётов.
4) Расчитать освещённости патчей пространства с помощью метода CalculateRadiosity() в классе RadiosityService, указав условия выхода из алгоритма (количество шагов и/или минимальный выделенный+отражённый свет одним патчем).

### Пример использования в Unity:
![image](https://user-images.githubusercontent.com/42946134/235921940-da7e6c31-101a-43dc-8982-109225e209c9.png)
![image](https://user-images.githubusercontent.com/42946134/235922421-00e987af-f1ee-4db7-8d54-4ee9fd556988.png)  

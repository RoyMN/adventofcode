using Extensions;
using static System.Console;

//Part1();
Part2();

void Part1()
{
    string input = File.ReadAllText("./input");
    string[] steps = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

    int sum = 0;
    foreach (var step in steps)
    {
        sum += step.Hash();
    }
    WriteLine(sum);
}

void Part2()
{
    string input = File.ReadAllText("./input");
    string[] steps = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

    var boxes = new List<Label>[256];
    for (int i = 0; i < 256; i++)
    {
        boxes[i] = new List<Label>();
    }

    foreach (var step in steps)
    {
        ProcessStep(step, boxes);
    }

    long focusingPowerSum = CalculateFocusingPower(boxes);
    WriteLine($"Total Focusing Power: {focusingPowerSum}");
}

static void ProcessStep(string step, List<Label>[] boxes)
{
    char operation = step.IndexOf('-') != -1 ? '-' : '=';
    string labelName = step.Split(operation)[0];
    int boxIndex = labelName.Hash() % 256;

    switch (operation)
    {
        case '-':
            int indexToRemove = boxes[boxIndex].FindIndex(label => label.Name == labelName);
            if (indexToRemove != -1)
            {
                boxes[boxIndex].RemoveAt(indexToRemove);
            }
            break;
        case '=':
            string[] parts = step.Split(operation);
            int focalLength = int.Parse(parts[1]);
            int indexToUpdate = boxes[boxIndex].FindIndex(label => label.Name == labelName);
            if (indexToUpdate != -1)
            {
                boxes[boxIndex][indexToUpdate].FocalLength = focalLength;
            }
            else
            {
                boxes[boxIndex].Add(new Label { Name = labelName, FocalLength = focalLength });
            }
            break;
    }
}

//             WriteLine($"{label.Name}: {boxNum + 1} (box {boxNum}) * {slotNum} * {label.FocalLength} (focal length)) = {partial}");

static long CalculateFocusingPower(List<Label>[] boxes)
{
    long sum = 0;
    for (int boxNum = 0; boxNum < boxes.Length; boxNum++)
    {
        for (int slotNum = 0; slotNum < boxes[boxNum].Count; slotNum++)
        {
            var label = boxes[boxNum][slotNum];
            var partial = (long)(boxNum + 1) * (slotNum + 1) * label.FocalLength;
            WriteLine($"{label.Name}: {boxNum + 1} (box {boxNum}) * {slotNum + 1} (slot number) * {label.FocalLength} (focal length)) = {partial}");
            sum += partial;
        }
    }
    return sum;
}

class Label
{
    public string Name { get; set; }
    public int FocalLength  { get; set; }
}
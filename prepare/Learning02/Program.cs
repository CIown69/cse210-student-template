using System;

class Program
{
    static void Main(string[] args)
    {
        Job job1 = new Job();
        job1._jobTitle = "Receptionist";
        job1._company = "Jones Wilson LLP";
        job1._startYear = 2020;
        job1._endYear = 2021;

        Job job2 = new Job();
        job2._jobTitle = "Account Manager";
        job2._company = "DHL";
        job2._startYear = 2024;
        job2._endYear = 2024;

        Resume myResume = new Resume();
        myResume._name = "Connor Wilson";

        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        myResume.Display();
    }
}
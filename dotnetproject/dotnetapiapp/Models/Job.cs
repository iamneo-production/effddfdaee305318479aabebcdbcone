using System;
using System.Collections.Generic;

namespace BookStoreDBFirst.Models;
public class Job
{
    public int JobID { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public string Location { get; set; }
    public string Responsibility { get; set; }
    public string Qualification { get; set; }
    public DateTime DeadLine { get; set; }
}

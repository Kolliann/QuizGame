
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IQuestion
{
    public string Question();

    public List<IAnswear> GetAnswears();

    public string GetCorrectAnswear();
}


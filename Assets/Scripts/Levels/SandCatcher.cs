using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class SandCatcher : MonoBehaviour
{
    public SandGridManager grid;

    private void OnParticleCollision(GameObject other)
    {
        Vector3 hitPos = other.transform.position;
        grid.AddSandAt(hitPos);
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universe: MonoBehaviour
{
    // Start is called before the first frame update

    int meshSize = 50;
    int universeSize = 100;
    int dx;
    double[] density;
    double[] velocity;
    double[] gpot;

    double[] newDensity;
    double[] newVelocity;
    double[] newgpot;

    GameObject[] display;
    void Start()
    {
        dx = universeSize/meshSize;


        density = new double[meshSize];
        velocity = new double[meshSize];
        gpot = new double[meshSize];
        newDensity = new double[meshSize];
        newVelocity = new double[meshSize];
        newgpot = new double[meshSize];
        for(int i = 0; i < meshSize; i++)
        {
            density[i] = 0.1;
            velocity[i] = 0;
            gpot[i] = 0;
        }

        display = new GameObject[meshSize];
            // CubeプレハブをGameObject型で取得
            GameObject obj = (GameObject)Resources.Load("Cube");
        for(int i = 0; i < meshSize; i++)
        {
            // Cubeプレハブを元に、インスタンスを生成、
            display[i] = Instantiate(obj, new Vector3(i * universeSize / meshSize, 1.0f, 0.0f), Quaternion.identity);
            display[i].transform.localScale = new Vector3(10*(float)density[i], 10*(float)density[i], 10*(float)density[i]);
        }

        Debug.Log("init");
        foreach(var d in density)
        {
            Debug.Log(d);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < meshSize; i++)
        {

            //update potential
            if (i != 0 && i != meshSize - 1)
            {
                newgpot[i] = (gpot[i - 1] + gpot[i + 1] + 4 * Math.PI * dx * dx * density[i]) / 2;
            }

            //update velocity
            if (i == 0)
            {
                newVelocity[i] = 0.01*((gpot[i] + gpot[i]) - (gpot[i] + gpot[i+1])) / 2;
            }
            else if (i == meshSize - 1)
            {
                newVelocity[i] = 0.01*((gpot[i-1] + gpot[i]) - (gpot[i] + gpot[i])) / 2;
            }
            else
            {
                newVelocity[i] = 0.01*((gpot[i-1] + gpot[i]) - (gpot[i] + gpot[i+1])) / 2;
            }

            //update density
            if (i == 0)
            {
                newDensity[i] = density[i] + 0.01*(velocity[i] * density[i] - velocity[i + 1] * density[i + 1]) / 2;
            }
            else if (i == meshSize - 1)
            {
                newDensity[i] = density[i] + 0.01*(velocity[i - 1] * density[i - 1] - velocity[i] * density[i]) / 2;
            }
            else
            {
                newDensity[i] = density[i] + 0.01*(velocity[i - 1] * density[i - 1] - velocity[i + 1] * density[i + 1]) / 2;
            }
                if(newDensity[i] < 0)
                {
                    newDensity[i] = density[i];
                }
                if(newDensity[i] > 10)
                {
                    newDensity[i] = density[i];
                newVelocity[i - 1] += -10;
                newVelocity[i + 1] += 10;
                }
        }
        gpot = newgpot;
        density = newDensity;
        velocity = newVelocity;

        for(int i = 0; i < meshSize; i++)
        {
            display[i].transform.localScale = new Vector3(10*(float)density[i], 10*(float)density[i], 10*(float)density[i]);
        }

        Debug.Log("updated");
        foreach(var d in density)
        {
            Debug.Log(d);
        }
    }
}

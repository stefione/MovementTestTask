using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UpdateRacersExampleFunction : MonoBehaviour
{
    public class Racer
    {
        public bool IsAlive()
        {
            return true;
        }

        public void Update(float deltaTime)
        {

        }

        public bool IsCollidable()
        {
            return true;
        }

        public bool CollidesWith(Racer racer)
        {
            return true;
        }

        public void Destroy()
        {

        }
    }

    void UpdateRacers(float deltaTimeS, List<Racer> racers)
    {
        //We change this to HasSet to gain a bit of perfomance since it wont add duplicate racers.
        HashSet<Racer> racersNeedingRemoved = new HashSet<Racer>();
        // Updates the racers that are alive
        for (int i = 0; i < racers.Count; i++)
        {
            if (racers[i].IsAlive())
            {
                //Racer update takes milliseconds
                racers[i].Update(deltaTimeS * 1000.0f);
            }
        }

        ///For the collision function instead of comparing every racer to every other, we should make a collision matrix of some sort
        ///The collision matrix might be just the play area presented as a matrix, where every racer belongs to some field inside of matrix,
        ///that way we must only compare to the racers inside the field and the racers in the surrounding fields.
        ///More optimized version would be to use a QuadTree to have less fields that we need to check, and to reduce the number of checks.
        ///QuadTree will enable us to create smaller and smaller fields depending on the dencity of racers 
        ///and areas where there is a few racers or non to be large.
        ///Additionaly if we have an infinite world, we can adapt the QuadTree and matrix to resize of course but a simpler way would be to use a Hashtable
        ///With a Hashtable we can create keys from positions in the world. Beacuse positions can vary a lot we can normalize the positions to fit inside of set areas
        ///by deviding the positions and rounding the numbers to integers so for example all positions between 0-100 will fit inside of index 0 if we divide by 100 and
        ///from 101-200 will be index 1. This way we create a sparse matrix where some fields exist and some dont.
        ///In every case we need to assign every racer to the correct field, and update that infomation after every position change.

        // Collides
        //Since we already compared from number "i" we dont need to compare again in reverse.
        for (int i = 0; i < racers.Count; i++)
        {
            for (int j = i+1; j < racers.Count; j++)
            {
                Racer racer1 = racers[i];
                Racer racer2 = racers[j];
                if (racer1.IsCollidable() && racer2.IsCollidable() && racer1.CollidesWith(racer2))
                {
                    OnRacerExplodes(racer1);
                    racersNeedingRemoved.Add(racer1);
                    racersNeedingRemoved.Add(racer2);
                }
            }
        }
        // Get rid of all the exploded racers
        foreach(var racer in racersNeedingRemoved)
        {
            racer.Destroy();
            racers.Remove(racer);
        }

        //The "racers" list has the remaining Alive racers
    }

    public void OnRacerExplodes(Racer r)
    {

    }
}

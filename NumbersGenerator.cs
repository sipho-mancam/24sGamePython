using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NumbersGenerator : MonoBehaviour
{
    private int[] mains = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public static HashSet<int> MAIN_SET = new HashSet<int>();

    private NumbersGen ng;
    // Start is called before the first frame update
    void Start()
    {
        MAIN_SET = new HashSet<int>(mains);
        ng = new NumbersGen(new HashSet<int>(mains));
    }

    // Update is called once per frame
    void Update()
    {
        ng.run();
    }
}

public class Output
{

    public int x, y;
    public List<object> i_j;
    
    public Output(int x, int y, List<object> j)
    {
        this.x = x;
        this.y = y;
        this.i_j = j;
    }
}

public class NumbersGen
{

    private HashSet<Vector2> multiples = new HashSet<Vector2>();
    private HashSet<Vector2> r_multiples = new HashSet<Vector2>();
    private HashSet<int> main_set, running_set;

    public NumbersGen(HashSet<int> data_set)
    {
        this.main_set = data_set;
        this.multiples.Add(new Vector2(1, 24));
        this.multiples.Add(new Vector2(2, 12));
        this.multiples.Add(new Vector2(3, 8));
        this.multiples.Add(new Vector2(4, 6));

        this.r_multiples.Add(new Vector2(24, 1));
        this.r_multiples.Add(new Vector2(12, 2));
        this.r_multiples.Add(new Vector2(8, 3));
        this.r_multiples.Add(new Vector2(6, 4));
    }

    private HashSet<int> remove_entry(int n, HashSet<int> set_n)
    {
        // create a copy of the object in memory to not modify the original
        HashSet<int> new_set = new HashSet<int>(set_n);

        if(new_set.Contains(n))
        {
            new_set.Remove(n);
            return new_set;
        }
        else
        {
            return new_set;
        }
                                           
    }

    private HashSet<int> runTest(int in_v, int target, HashSet<int> test_set)
    {

        int j = 0;

        HashSet<int> output_set = new HashSet<int>();

        j = Mathf.Abs(in_v - target);

        if(test_set.Contains(j))
        {
            output_set.Add(j);
        }

        j = target / in_v;
        if (target%in_v == 0 && test_set.Contains(j))
        {
            output_set.Add(j);
        }

        j = in_v / target;
        if (in_v%target == 0 && test_set.Contains(j))
        {
            output_set.Add(j);
        }

        j = target * in_v;
        if (test_set.Contains(j))
        {
            output_set.Add(j);
        }

        return output_set;
    }

    private List<object> generateCombos(Vector2 input_pair, Vector2 target_pair, HashSet<int> test_set)
    {
        HashSet<int> fixed_set = test_set;
        List<object> output_list = new List<object>();
        //# fixed set is the set without x, and y in it.
        //# first generate the list of j's in the (x, y, j, i) - > combination
        //# but the j's can only be valid when they have corresponding i's
        int j_input = (int)input_pair.x;
        int j_target = (int)target_pair.x;

        HashSet<int> j_set = this.runTest(j_input, j_target, fixed_set);

        //# Get the I's for the corresponding J
        //# Remember: J only affects the set, not the computation.
        int i_input = (int)input_pair.y;
        int i_target = (int)target_pair.y;

        foreach(int j in j_set)
        {
            HashSet<int> temp_set = this.remove_entry(j, fixed_set);
            HashSet<int> i_jth_set = this.runTest(i_input, i_target, temp_set);

            if (i_jth_set.Count > 0)
            {
                List<object> temp = new List<object>();
                temp.Add(j);
                temp.Add(new List<int>(i_jth_set));
                output_list.Add(temp);
            }
        }
        return output_list;
    }

    private List<Output> generateAllCombinations(Vector2 input_pair)
    {
        List<Output> output = new List<Output>();
        List<object> res;
        foreach(Vector2 target_pair in this.multiples)
        {
            //# the running set is the complete set without the x, y inputs that were chosen at random

            res = this.generateCombos(input_pair, target_pair, this.running_set);
            if(res.Count > 0)
            {
                Output temp = new Output((int)input_pair.x, (int)input_pair.y, res);
                output.Add(temp);
            }
        }

        foreach (Vector2 target_pair in this.r_multiples)
        {
            //# the running set is the complete set without the x, y inputs that were chosen at random
            res = this.generateCombos(input_pair, target_pair, this.running_set);
            if (res.Count > 0)
            {
                Output temp = new Output((int)input_pair.x, (int)input_pair.y, res);
                output.Add(temp);
            }
        }

        return output;
    }

    public List<int> run()
    {
        List<int> list = new List<int>(this.main_set);

        var random = new System.Random();
        int index = random.Next(list.Count);

        int x = list[index];
        list.Remove(x);
        
        index = random.Next(list.Count);
        int y = list[index];
        
        list.Remove(y);

        running_set = new HashSet<int>(list);

        List<Output> output_space = this.generateAllCombinations(new Vector2(x, y));

       // Debug.Log(output_space[0].x+" , "+output_space[0].y+ " , "+ this.printList(output_space[0].i_j));

        index = random.Next(output_space.Count);

        Output out_set = output_space[index];

        List<int> output_ready = new List<int>();

        output_ready.Add(out_set.x);
        output_ready.Add(out_set.y);

        List<object> j_i = out_set.i_j;
        index = random.Next(j_i.Count);

        List<object> j_i_r = (List<object>) j_i[index];
        output_ready.Add((int) j_i_r[0]);

        List<int> i_th = (List<int>)j_i_r[1];
        index = random.Next(i_th.Count);

        output_ready.Add(i_th[index]);

        //  Debug.Log(this.printList(output_ready));

        return output_ready;
    }


    private string printList(List<int> l)
    {
        string s = "";

        foreach(var j in l)
        {
            s += " " + j;
        }

        return s;
    }






}

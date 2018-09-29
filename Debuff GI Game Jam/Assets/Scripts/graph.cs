	/*
	 example: 
		graph g = new graph();
	    graph.tree_node root = g.generate_tree(4,4,0.7f);
		Debug.Log(root.ToString());
	*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class graph{
	public class tree_node{
		public tree_node parent;
		public List<tree_node> children;
		public string name;
		public string ToString (){
			string s = this.name + "\n" ;
			for(int i=0;i<this.children.Count;i++){
				List<string> children_strings = children[i].ToString().Split('\n').ToList();
				for(int j=0;j<children_strings.Count;j++){
					s += " > " + children_strings[j] + "\n";
				}
			}
			//remove the last new line
			s = s.Substring(0, s.Length-1);
			return s;
		}
		public tree_node(tree_node parent, List<tree_node> children, string name){
			this.parent = parent;
			this.children = children;
			this.name = name;
		}
	}
	
	public tree_node generate_tree(int depth, int branch_factor, float chance_to_branch){
		System.Random rnd = new System.Random();
		tree_node root = new tree_node(null, new List<tree_node>(),rnd.Next(1, 100000).ToString());
		List<tree_node> this_level = new List<tree_node>();
		List<tree_node> next_level = new List<tree_node>();
		this_level.Add(root);
		for(int c = 0; c<depth;c++){ // do c times:		
			for(int i=0; i<this_level.Count;i++){ // for each current level:
				for(int j=0; j<branch_factor;j++){
					if(rnd.NextDouble() < chance_to_branch){ // if can branch:
						tree_node next = new tree_node(this_level[i], new List<tree_node>(), rnd.Next(1, 100000).ToString()); // might want to find a better way to name nodes.
						this_level[i].children.Add(next);
						next_level.Add(next);
					}
				}
			}
			this_level = next_level;
			next_level = new List<tree_node>();
		}
		return root;
	}
}
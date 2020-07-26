# Directory Structure 

## A good practice

A good practice for a directory structure - to keep things clean - is.
 * put each Command and its Parameters into a directory 
    * with the name of the Command
 * let all classes in this directory have the Namespace of the directory
 * Add two classes 
    * `class Command<Parameter>` and `class Parameters`
 
 Bottom line: each Command and its Parameters 'live' in their Namespace and the code lies in its own directory. 
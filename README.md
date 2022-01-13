# Pipeline

This repo has libraries for DotNet that implement the mediator, chain of responsibility, and decorator patterns in order to build a pipeline of functions (handlers) to process requests (such as commands and queries) and produce results (response). This is designed for functions (handlers) to be plugged in without any direct dependency.You can use a simple function delegate or determine that a method of a service will be a handler.

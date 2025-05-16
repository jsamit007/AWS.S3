using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;

namespace AWS.StepFunction;

public class StepFunctionService
{
    private readonly IAmazonStepFunctions _stepFunctionClient;

    public StepFunctionService(IAmazonStepFunctions stepFunctionClient)
    {
        _stepFunctionClient = stepFunctionClient;
    }

    public async Task<StartExecutionResponse> StartExecutionAsync(string stateMachineArn, string input)
    {
        var request = new StartExecutionRequest
        {
            StateMachineArn = stateMachineArn,
            Input = input,
            Name = Guid.NewGuid().ToString() // Optional: Generate a unique name for the execution,
        };

        return await _stepFunctionClient.StartExecutionAsync(request);
    }

    public async Task<DescribeExecutionResponse> DescribeExecutionAsync(string executionArn)
    {
        return await _stepFunctionClient.DescribeExecutionAsync(new DescribeExecutionRequest
        {
            ExecutionArn = "arn:aws:states:eu-north-1:480238144354:express:TestMachine1:df69c710-d8fc-4ef4-9077-14ec50264eef:7bc0a89c-680c-4f54-989f-af8cc2857689"
        });
    }
}

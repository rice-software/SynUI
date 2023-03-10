local httpService = game:GetService("HttpService")
local logService = game:GetService("LogService")
local player = game:GetService("Players").LocalPlayer

while true do
    local success, ws = pcall(syn.websocket.connect, "ws://localhost:7500")
    
    if success then
        local event = logService.MessageOut:Connect(function(message, messageType)
            pcall(function()
                ws:Send(httpService:JSONEncode({
                    name = player.Name,
                    message = message,
                    messageType = messageType.Name
                }))
            end)
        end)

        ws.OnClose:Wait()
        event:Disconnect()
    end

    task.wait(1000)
end
# Suki Chat AI机器人

Suki Chat AI机器人是一个独立于SukiChat服务器的分布式AI代理系统。该程序可以代替真实用户自动响应消息，支持多机器人同时运行，只需简单配置API密钥即可启用。

## 核心特性

- **分布式架构**：支持在多台服务器上部署多个机器人实例
- **AI驱动**：接入主流AI服务（OpenAI、Azure OpenAI、Anthropic等）
- **简易配置**：通过JSON配置文件即可完成所有设置
- **自动代理**：机器人可完全代替账号进行自动应答
- **多机器人管理**：单一实例可同时运行多个不同配置的机器人

## 使用方法

### 1. 安装与配置

1. 更改`appsettings.json`配置文件，填写机器人配置信息
3. 配置AI服务API密钥

### 2. 配置文件说明

```json
{
    // 数据存放地址
    "DataBasePath": "D:\\ChatResources\\RobotDb\\robot.db",
    // 机器人配置
    "Robot": [
        {
            "User": {
                "ID": "账号ID",
                "Password": "账号密码"
            },
            "API": {
                "Key": "API密钥",
                "URL": "AI请求地址"
            },
            "System": [
                "填写系统提示词_1",
                "填写系统提示词_2"
            ],
            "Temperature": 1.3,
            "AcceptFriendRequest": true
        },
        {
            "User": {
                "ID": "账号ID",
                "Password": "账号密码"
            },
            "API": {
                "Key": "API密钥",
                "URL": "AI请求地址"
            },
            "System": [
                "填写系统提示词_1",
                "填写系统提示词_2"
            ],
            "Temperature": 0,
            "AcceptFriendRequest": true
        }
    ]
}
```
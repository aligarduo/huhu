# huhu_api

## 简介
> 乎乎，高质量的创作者聚集的原创内容平台，让人们更好的分享知识、经验和见解，找到自己的解答，聚集了互联网科技、商业、影视、时尚、文化等领域最具创造力的人群的平台。

## 更新至 v2.0 说明

> 2021.10.30 更新至 2.0.0，部分api接口和参数做了调整，如还不想升级到 2.0.0，请查看 [v1 的文档](/more/v1)，[更新日志](/more/log)，同时 1.0 将不再维护，部分api可能无法正常访问。

## 接口文档

### 调用前须知

> 1005 错误基本都是没登录就调用了需要登录的 api

> 部分 api 如登录 api 不能调用太频繁，否则可能会触发 503 错误或者 ip 高频错误

> 分页 api 返回字段里 has_more 为 true 时，则有下一页

> 文档可能会有缓存，如果文档版本和 github 上的版本不一致，请清除缓存再查看

### 错误码

**0 表示成功，非 0 表示失败，失败错误码列表：**

| 错误码 | 说明 | 处理措施 |
| ---- | ---- | ------- |
| 0 | 成功 | - |
| 1 | 服务器内部错误 | 请联系 [huhu helper](http://mail.qq.com/cgi-bin/qm_share?t=qm_mailme&email=2966994290@qq.com) 沟通解决 |
| 1001 | sign缺少字段 | 请遵守 sign 说明规范 |
| 1002 | sign请求超时 | 请遵守 sign 说明规范 |
| 1003 | sign校验失败 | 请遵守 sign 说明规范 |
| 1004 | csrf_token校验失败 | token 已超过有效期，请重新登录 |
| 1005 | 请求没有权限 | 请检查请求标头是否有带上 **passport_csrf_token** |
| 1006 | 参数错误 | 请注意参数空与非空 |
| 1007 | 手机号格式错误 | - |
| 1008 | 验证码错误 | 请注意是否按要求加密 |
| 1009 | 该帐号因涉及异常操作被限制登录 | 违规 1 次限制 3 天，违规 2 次限制 15 天 |
| 1010 | 该帐号因涉及发布违规内容被限制登录 | 违规 1 次限制 3 天，违规 2 次限制 15 天 |
| 1011 | 该帐号因多次涉及违规行为被永久关闭 | 违规 3 次将永久关闭 |
| 1012 | 账号已被注销 | - |
| 1013 | 帐号或密码错误 | 请注意密码是否按要求加密 |
| 1014 | 不支持该请求 | - |
| 1015 | 不支持此链接 | 请将链接内容下载到本地后，再上传至乎乎获取同域链接 |
| 1016 | 此链接无法正常访问 | 请检查此链接是否可以正常访问 |
| 1017 | 内容为空 | - |
| 1018 | 此收藏集名称已存在 | - |
| 1019 | 此收藏集不存在，请先创建收藏集 | - |
| 1020 | 此收藏集下不存在该项目 | - |
| 1021 | 请勿重复操作 | - |
| 1022 | 操作失败，请先进行添加项目 | - |
| 1023 | 草稿不存在 | - |
| 1024 | 类别不存在 | - |
| 1025 | 标签不存在 | - |
| 1026 | 邮箱格式错误 | - |
| 1027 | 仅支持jpg、png格式 | - |
| 1028 | 文章不存在或已被删除 | - |
| 1029 | 评论不存在或已被删除 | - |
| 1030 | 回复不存在或已被删除 | - |
| 1031 | 用户不存在 | - |
| 1032 | 操作类型错误 | - |
| 1033 | 类别名称已存在 | - |
| 1034 | 类别跳转地址已存在 | - |
| 1035 | 标签名称已存在 | - |
| 1036 | 手机号已存在 | - |
| 1037 | 验证码下发失败 | 请稍后重试 |
| 1038 | 话题圈子名称已存在 | - |
| 1039 | 话题圈子不存在 | - |
| 1040 | 话题不存在 | - |
| 1041 | 选项已存在 | - |
| 1042 | 选项不存在 | - |
| 1043 | 对象不能是自己 | - |
| 1044 | 此广告不存在 | - |
| 1045 | 此管理员不存在 | - |
| 1046 | 此管理员账号因违规被超级管理员关闭 | - |

### 安全签名

防止数据传输过程中被拦截篡改

**请求标头（Request Header）**

|  属性 | 类型 | 必填 |说明 |
| ----- | ----- | ---- | ---- |
| appid  | number | 是 | [应用接入ID](/more/sdk?id=应用接入id) | 
| timestamp  | number | 是 | 时间戳（13位）| 
| nonce  | number | 是 | 随机数（6位）| 
| sign  | string | 是 | 加密签名字符串（[使用32位MD5加密](/more/sdk?id=使用32位MD5加密)）| 

[sign 签名规则请查看 SDK 文档](/more/sdk?id=安全签名规则)

### Token认证
Token 是服务端颁发给客户端的一个令牌，当第一次登录后，服务端生成一个 Token 便将此 Token 返回给客户端，以后客户端只需带上这个 Token 前来请求数据即可，无需再次带上用户名和密码。

**请求标头（Request Header）**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| passport_csrf_token | string | 是 | token 令牌 |

### 验证码

#### 手机登录验证码  
**备注：**【乎乎】的腾讯云短信接口暂为个人认证，权限有所限制  
- 🔒 对同一个手机号，30 秒内发送短信条数不超过 1 条  
- 🔒 对同一个手机号，1 小时内发送短信条数不超过 5 条  
- 🔒 对同一个手机号，1 自然日内发送短信条数不超过 10 条  

[安全签名认证](#安全签名)：`是`

**请求URL :** `https://api.huhu.chat/passport/web/send_code`

**请求方式 :** `GET`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
| ------------------ | -- | ------ | --------------------|
| account_sdk_source | string | 是 | 客户端请求类型（ web、android ）|
| mobile | string | 是 | 国际区号+11位手机号（ 仅支持中国大陆'**+86**' ）|

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {
        "expires": "5",  //有效时长（单位：分钟）
        "start_time": "2021-10-10 12:40:17",  //生效时间
        "expire_time": "2021-10-10 12:45:17"  //到期时间
    }
}
```

#### 邮箱绑定验证码

[安全签名认证](#安全签名)：`是`

**请求URL :** `https://api.huhu.chat/passport/web/send_email_code`

**请求方式 :** `GET`

**请求参数**

| 参数名 | 类型 | 必选 | 说明 |
| ------------------ | -- | ------ | --------------------|
| account_sdk_source | string | 是 | 客户端请求类型（ web、android ）|
| email | string | 是 | 邮箱地址（ 仅支持QQ邮箱 ） |

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {
        "expires": "5",  //有效时长（单位：分钟）
        "start_time": "2021-10-10 13:13:11",  //生效时间
        "expire_time": "2021-10-10 13:18:11"  //到期时间
    }
}
```

### 登录

#### 手机登录

[安全签名认证](#安全签名)：`是`

**请求URL :** `https://api.huhu.chat/passport/web/sms_login`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-------|----|--------|---------|
| mobile | string | 是 | 国际区号+11位手机号（ 仅支持中国大陆'**+86**' ）|
| code | number | 是 | 4位验证码（ [使用8位MD5加密](/more/sdk?id=使用8位MD5加密) ） |

**请求示例 :**
```json
{
    "mobile": "+8612345678900",
    "code": 1234
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "token": "token"
}
```

#### 密码登录

[安全签名认证](#安全签名)：`是`

**请求URL :** `https://api.huhu.chat/passport/web/cipher_login`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| account | string | 是 | 11位手机号或QQ邮箱地址 |
| password | string | 是 | 登录密码（ [使用16位MD5加密](/more/sdk?id=使用16位md5加密) ） |

**请求示例 :**
```json
{
    "account": "12345678900",
    "password": "ea8a706c4c34a168"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "token": "token"
}
```

#### 退出登录

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/passport/web/logout`

**请求方式 :** `POST`

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```










### 扫码登录

#### 获取二维码

**请求URL :** `https://api.huhu.chat/passport/web/richscan`

**请求方式 :** `GET`

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {
        "uuid": 4672928302056115259, //UUID
        "expires": "180", //有效时间（秒）
        "start_time": "2021-12-14 10:48:14", //开始时间
        "expire_time": "2021-12-14 10:51:14" //到期时间
    }
}
```

#### 刷新状态

**请求URL :** `https://api.huhu.chat/passport/web/richscan_connect`

**请求方式 :** `GET`

**请求示例 :** `https://api.huhu.chat/passport/web/richscan_connect?uuid=123456789`

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {
        "uuid": "123456789",
        "code": 1047,
        "msg": "二维码未失效"
    }
}
```

### 用户信息

#### 获取个人信息

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/user_api/v1/user/get`

**请求方式 :** `GET`

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {} //个人信息
}
```

#### 更新个人信息

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/user_api/v1/user/update`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| user_name | string | 选填 | 昵称 |
| avatar_large | url | 选填 | URL链接（必须经过[图片处理](#上传图片)） |
| password | string | 选填 | 登录密码（ [使用16位md5加密](/more/sdk?id=使用16位md5加密) ）|

**请求示例 :**
```json
{
    "user_name": "新昵称"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data":{} //更新后的个人信息
}
```

### 图片处理

#### 上传图片

**请求URL :** `https://api.huhu.chat/image/get_img_url?method=banzoom&wmark=false`

**请求方式 :** `POST`

**请求类型 :** `form-data`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| method | string | 是 | 方式（ original、cover、profile ） |
| wmark | bool | 是 | 水印（ true、false ） |

**说明 :**  
`original`：原图（规格不变，不压缩）  
`cover`：封面（压缩为 240x160 像素）  
`profile`：头像（压缩为 300x300 像素）  

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {
        "main_url": "",  //主链接
        "backup_url": ""  //备用链接
    }
}
```

### 文章标签

#### 按全部标签分页获取

**请求URL :** `https://api.huhu.chat/tag_api/v1/query_tag_list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //标签数据
    "has_more": true  //是否还有下一页
}
```

### 草稿

#### 创建草稿

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article_draft/create`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| title | string | 是 | 标题 |
| brief_content | string | 是 | 摘要 |
| mark_content | string | 是 | 内容 |
| cover_image | url | 是 |封面URL链接（必须经过[图片处理](#上传图片)） |
| tag_ids | list | 是 | 标签 |
| edit_type | string | 是 | 编辑器类型（ md、rich_text ） |

**请求示例 :**
```json
{
    "title": "标题",
    "brief_content": "摘要",
    "mark_content": "内容",
    "cover_image": "封面URL",
    "tag_ids": [123456,123456],
    "edit_type": "编辑器类型"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}  //草稿信息
}
```

#### 删除草稿

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article_draft/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| draft_id | number | 是 | 草稿ID |

**请求示例 :**
```json
{
    "draft_id": 13800138000
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 更新草稿

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article_draft/update`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| draft_id | number | 是 | 草稿ID |
| title | string | 是 | 标题 |
| brief_content | string | 是 | 摘要 |
| mark_content | string | 是 | 内容 |
| cover_image | url | 是 |封面URL链接（必须经过[图片处理](#上传图片)） |
| tag_ids | list | 是 |标签 |
| edit_type | string | 是 | 编辑器类型（ md、rich_text ）|

**请求示例 :**
```json
{
    "draft_id": 13800138000,
    "title": "标题",
    "brief_content": "摘要",
    "mark_content": "内容",
    "cover_image": "封面URL",
    "tag_ids": [123456],
    "edit_type": "编辑器类型"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {} //草稿信息
}
```

#### 查看草稿详细

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article_draft/detail`

**请求方式 :** `POST`

请求参数

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| draft_id | number | 是 | 草稿ID |

**请求示例 :**
```json
{
    "draft_id": 13800138000
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {} //草稿信息
}
```

#### 全部草稿分页获取

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article_draft/query_list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //标签数据
    "has_more": true  //是否还有下一页
}
```

### 文章

#### 发布文章

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article/publish`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| draft_id | number | 是 | 草稿ID |
| title | string | 是 | 标题 |
| brief_content | string | 是 | 摘要 |
| mark_content | string | 是 | 内容 |
| cover_image | url | 是 |封面URL链接（必须经过[图片处理](#上传图片)） |
| tag_ids | list | 是 |标签 |
| edit_type | string | 是 | 编辑器类型（ md、rich_text ） |

**请求示例 :**
```json
{
    "draft_id": 13800138000,
    "title": "标题",
    "brief_content": "摘要",
    "mark_content": "内容",
    "cover_image": "封面",
    "tag_ids": [123456],
    "edit_type": "编辑器类型"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {} //文章信息
}
```

#### 删除文章

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/article/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| article_id | number | 是 | 文章ID |

**请求示例 :**
```json
{
    "article_id": 13800138000
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 更新文章

[Token认证](#Token认证)：`可选`

**请求URL :** `https://api.huhu.chat/content_api/v1/article/update`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| article_id | number | 是 | 文章ID |
| title | string | 是 | 标题 |
| brief_content | string | 是 | 摘要 |
| mark_content | string | 是 | 内容 |
| cover_image | url | 是 |封面URL链接（必须经过[图片处理](#上传图片)） |
| tag_ids | list | 是 |标签 |
| edit_type | string | 是 | 编辑器类型（ md、rich_text ） |

**请求示例 :**
```json
{
    "article_id": 1000000001,
    "title": "标题",
    "brief_content": "摘要",
    "mark_content": "内容",
    "cover_image": "封面URL",
    "tag_ids": [123456,123456],
    "edit_type": "编辑器类型"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {} //文章信息
}
```

#### 查看文章详细

[Token认证](#Token认证)：`可选`

**请求URL :** `https://api.huhu.chat/content_api/v1/article/detail`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| article_id | number | 是 | 文章ID |

**请求示例 :**
```json
{
    "article_id": 13800138000
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {
        "article_id": "13800138000", //文章ID
        "article_info": {}, //文章信息
        "tags": [], //文章标签
        "author_user_info": {}, //作者信息
        "category": {}, //文章分类
        "user_interact": {} //用户交互
    }
}
```

#### 文章推荐分页获取

[Token认证](#Token认证)：`可选`

**请求URL :** `https://api.huhu.chat/recommend_api/v1/article/recommend_all_feed`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "20",  //光标所在位置
    "data": [],  //数据
    "has_more": true  //是否还有下一页
}
```


#### 用户所发表的文章分页获取

[Token认证](#Token认证)：`可选`

**请求URL :** `https://api.huhu.chat/content_api/v1/article/query_list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |
| user_id | number | 是 | 用户ID |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20,
    "user_id": 1000000001,
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //数据
    "has_more": true  //是否还有下一页
}
```

### 评论

#### 发表评论

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/comment/publish`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| comment_content |string | 是 | 评论内容 |
| item_id | 是 | number | 被评论的项目ID |

**请求示例 :**
```json
{
    "comment_content": "👏👏",
    "item_id": 3965817924773478400
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}
}
```

#### 删除评论

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/comment/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| comment_id | string | 是 | 评论ID |

**请求示例 :**
```json
{
    "comment_id": "3965822329438601216"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 获取评论列表

**请求URL :** `https://api.huhu.chat/interact_api/v1/comment/list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |
| item_id | string | 是 | 文章ID |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20,
    "item_id": "3965817924773478400"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "20",  //光标所在位置
    "data": [
        {
            "comment_id": "3965825192709586944",  //评论ID
            "comment_info": {},  //评论信息
            "reply_infos": [
                {
                    "reply_id": "3965904859764359168",  //回复ID
                    "reply_info": {}, //回复信息
                    "user_info": {},  //发布回复的用户信息
                    "reply_user": {}, //被回复的回复的用户信息
                    "user_interact": {}  //用户交互
                }
            ],
            "user_info": {},  //发布评论的用户信息
            "user_interact": {}  //用户交互
        }
    ],
    "has_more": true  //是否还有下一页
}
```

### 回复

#### 发表回复

**请求URL :** `https://api.huhu.chat/interact_api/v1/reply/publish`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 可为NULL | 说明 |
|------|-----|------|------|-----|
| item_id | string | 是 | 否 | 文章ID |
| reply_content | string | 是 | 否 | 回复内容 |
| reply_to_comment_id | string | 是 | 否 | 回复的评论id |
| reply_to_reply_id | string | 选填 | 是 | 回复的回复id |
| reply_to_user_id | string | 选填 | 是 | 回复的回复用户id |

**请求示例 :**
```json
{
    "item_id": "3965817924773478400",
    "reply_content": "😍",
    "reply_to_comment_id": "3965825192709586944",
    "reply_to_reply_id": null,
    "reply_to_user_id": null
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}
}
```

#### 删除回复

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/reply/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| comment_id | string | 是 | 回复的评论ID |
| reply_id | string | 是 | 回复的ID |

**请求示例 :**
```json
{
    "comment_id": "3965825192709586944",
    "reply_id": "3965904794534543360"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

### 点赞

#### 加入点赞

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/digg/save`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| item_id | string | 是 | 点赞的项目ID |
| item_type | number | 是 | 点赞的类型：(1)点赞文章 (2)点赞评论 (3)点赞回复 |

**请求示例 :**
```json
{
    "item_id": "3965817924773478400",
    "item_type": 1
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 取消点赞

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/digg/cancel`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| item_id | string | 是 | 点赞的项目ID |
| item_type | number | 是 | 点赞的类型：(1)点赞文章 (2)点赞评论 (3)点赞回复 |

**请求示例 :**
```json
{
    "item_id": "3965817924773478400",
    "item_type": 1
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

### 关注

#### 加入关注

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/follow/do`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| id | string | 是 | 关注的ID |
| type | number | 是 | 关注的类型：(1)作者 |

**请求示例 :**
```json
{
    "id": "1450278043",
    "type": 1
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 取消关注

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`


**请求URL :** `https://api.huhu.chat/interact_api/v1/follow/undo`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| id | string | 是 | 关注的ID |
| type | number | 是 | 关注的类型：(1)作者 |

**请求示例 :**
```json
{
    "id": "3965817924773478400",
    "type": 1
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

### 收藏集

#### 创建收藏集

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collection/add`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| collection_name | string | 是 | 收藏集名称 |

**请求示例 :**
```json
{
    "collection_name": "收藏集名称"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}  //收藏集信息
}
```

#### 删除收藏集

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collection/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| collection_id | string | 是 | 收藏集ID |

**请求示例 :**
```json
{
    "collection_id": "3965921287670333440"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 获取收藏集列表

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collection/list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |
| article_id | string | 是 | 文章ID |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20,
    "article_id": 123456789
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //收藏集数据
    "has_more": true  //是否还有下一页
}
```

#### 更新收藏集

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collection/update`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| collection_id | string | 是 | 收藏集ID |
| collection_name | string | 是 | 修改后的收藏集名称 |

**请求示例 :**
```json
{
    "collection_id": "3965925388311330816",
    "collection_name": "修改后的收藏集名称"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}  //修改后的收藏集信息
}
```

### 收藏

#### 加入收藏

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collect/add`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| collection_id | string | 是 | 收藏集ID |
| item_id | string | 是 | 加入收藏的项目ID |

**请求示例 :**
```json
{
    "collection_id": "3965925388311330816",
    "item_id": "3965817924773478400"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 取消收藏

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collect/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| collection_id | string | 是 | 收藏集ID |
| item_id | string | 是 | 加入收藏的项目ID |

**请求示例 :**
```json
{
    "collection_id": "3965925388311330816",
    "item_id": "3965817924773478400"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

#### 获取收藏列表

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/interact_api/v1/collect/list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit |  number| 是 | 数据量 |
| collection_id | string | 是 | 收藏集ID |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20,
    "collection_id": "3962559341768212480"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success"
}
```

### 话题

#### 发表话题

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/topic_api/v1/topic/publish`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| topic_circle_id | string | 是 | 话题圈子ID |
| content | string | 是 | 内容 |
| pic_list | list | 是 | 图片集URL链接（必须经过[图片处理](#上传图片)） |
| url | url | 是 | 分享链接 |

**请求示例 :**
```json
{
    "topic_circle_id": "",
    "content": " #iPhone13值得买吗# 啊哈哈哈😁 #AA# 呜呜呜",
    "pic_list": [],
    "url": ""
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}  //话题信息
}
```

#### 删除话题

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/topic_api/v1/topic/delete`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| topic_id | string | 是 | 话题圈子 |

**请求示例 :**
```json
{
    "topic_id": "3968302477169655808"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
}
```

#### 指定搜索

[安全签名认证](#安全签名)：`否`

[Token认证](#Token认证)：`否`

**请求URL :** `https://api.huhu.chat/topic_api/v1/topic/list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |
| sort_type | number | 是 | 筛选类型 (1)圈子ID、(2)话题关键字 |
| sort_key | string | 是 | 圈子ID 或 话题关键字 |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 10,
    "sort_type": 1,
    "sort_key": "0"
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //数据
    "has_more": true  //是否还有下一页
}
```

#### 更新话题

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/topic_api/v1/topic/update`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| topic_id | string | 是 | 话题ID |
| topic_circle_id | string | 是 | 话题圈子ID |
| content | string | 是 | 内容 |
| pic_list | list | 是 | 图片集URL链接（必须经过[图片处理](#上传图片)） |
| url | url | 是 | 分享链接 |

**请求示例 :**
```json
{
    "topic_id": "3968301207323148288",
    "topic_circle_id": "",
    "content": "#iPhone13值得买吗# 啊哈哈哈😁",
    "pic_list": [],
    "url": ""
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": {}  //更新后的话题信息
}
```

### 全局搜索

#### 搜索

[安全签名认证](#安全签名)：`否`

[Token认证](#Token认证)：`否`

**请求URL :** `https://api.huhu.chat/search_api/v1/search`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |
| key_word | string | 是 | 搜索关键字 |
| id_type | number | 是 | 搜索类型 (1)文章、(2)话题 |
| sort_type | number | 是 | 筛选类型 (1)综合排序、(2)最新优先 |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20,
    "key_word": "1",
    "id_type": 1,
    "sort_type": 2
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //数据
    "has_more": true  //是否还有下一页
}
```

### 广告

#### 获取广告列表

**请求URL :** `https://api.huhu.chat/content_api/v1/advert/query_adverts`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| rank | number | 是 | 等级（10-100） |
| layout | number | 是 | 模板：(1) 2条数据、(2) 3条数据）|

**请求示例 :**
```json
{
    "rank": 98,
    "layout": 1
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "count": 100,  //数据量
    "cursor": "10",  //光标所在位置
    "data": [],  //广告数据
    "has_more": true  //是否还有下一页
}
```

### 举报

#### 发表

[安全签名认证](#安全签名)：`否`

[Token认证](#Token认证)：`否`

**请求URL :** `https://api.huhu.chat/report_api/v1/report/publish`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| issue | string | 是 | 举报问题选项ID |
| article_id | string | 是 | 文章ID |
| prove_pic | string | 是 | 截图URL链接（必须经过[图片处理](#上传图片)） |
| describe | string | 是 | 描述 |

**请求示例 :**
```json
{
    "issue": "3968408911387557888",
    "article_id": "3964750716244852736",
    "prove_pic": [],
    "describe": ""
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": []
}
```

### 反馈建议

#### 发表反馈

[安全签名认证](#安全签名)：`是`

[Token认证](#Token认证)：`是`

**请求URL :** `https://api.huhu.chat/content_api/v1/short_msg/publish`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| content | string | 是 | 内容 |
| pic_list | list | 是 | 图片集合 |

**请求示例 :**
```json
{
    "content": "",
    "pic_list": []
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": []
}
```

#### 获取反馈列表

**请求URL :** `https://api.huhu.chat/content_api/v1/short_msg/list`

**请求方式 :** `POST`

**请求参数**

| 属性 | 类型 | 必填 | 说明 |
|-----|----|----|------|
| cursor | string | 是 | 索引 |
| limit | number | 是 | 数据量 |

**请求示例 :**
```json
{
    "cursor": "0",
    "limit": 20
}
```

**响应示例 :**
```json
{
    "err_no": 0,
    "err_msg": "success",
    "data": []
}
```

## 关于此文档

此文档由 [docsify](https://github.com/docsifyjs/docsify) 生成。docsify 是一个动态生成文档网站的工具。不同于 GitBook、Hexo 的地方是它不会生成将 .md 转成 .html 文件，所有转换工作都是在运行时进行。
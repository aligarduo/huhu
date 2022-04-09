import json
import requests
import urllib.parse
import re

# 协议头
headers = {
    "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)\
     Chrome/94.0.4606.61 Safari/537.36 Edg/94.0.992.31"
}

token = {
    "passport_csrf_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzZXRJZCI6InRva2VuOjc0YTcyNDVjOTQzMmZkNTIiLCJzZXRQYXJhbXMiOiIyOTY2OTk0MjkwIiwic2V0SXNzdWVyIjoiaHVodSIsInNldElzc3VlZEF0IjoiMTYzNTY1NTcwOSIsInNldEV4cGlyYXRpb24iOiIxNjM3MzgzNzA5In0.hF_Smxa2LtKO-MRUic4AoJLnKz42ZMCXKMQZaqSmagY"
}

# 请求参数
param = {
    "url_recommend": "https://api.juejin.cn/recommend_api/v1/article/recommend_cate_tag_feed",
    "url_detail": "https://api.juejin.cn/content_api/v1/article/detail",
    "data": {
        "cate_id": "6809637771511070734",
        "cursor": "0",
        "id_type": 2,
        "limit": 20,
        "sort_type": 200,
        "tag_id": "6809640656265281550",
    }
}


# 发起第一次请求，获取文章id集合
def open_post():
    request = requests.post(
        url=param["url_recommend"],
        json={
            "id_type": param["data"]["id_type"],
            "sort_type": param["data"]["sort_type"],
            "cate_id": param["data"]["cate_id"],
            "tag_id": param["data"]["tag_id"],
            "cursor": param["data"]["cursor"],
            "limit": param["data"]["limit"]
        },
        headers=headers
    )
    response_json = json.loads(request.text)
    parse_data(response_json)


# 解析第一次请求，解析文章id集合
def parse_data(response_json):
    if response_json["err_no"] != 0:
        print("err_msg:", response_json["err_msg"])
    else:
        for i in range(len(response_json["data"])):
            article_info = response_json["data"][i]["article_id"]
            open_detail(article_info)


# 发起第二次请求，获取文章详细
def open_detail(article_ids):
    request = requests.post(
        url=param["url_detail"],
        json={
            "article_id": article_ids
        },
        headers=headers
    )
    response_json = json.loads(request.text)
    parse_detail(response_json)


# 解析第二次请求，解析文章详细
def parse_detail(response_json):
    if response_json["err_no"] != 0:
        print("err_msg:", response_json["err_msg"])
    else:
        article_info = response_json["data"]["article_info"]
        result = {
            "cover_image": article_info["cover_image"],
            "title": article_info["title"],
            "brief_content": article_info["brief_content"],
            "mark_content": article_info["mark_content"]
        }
        if result["cover_image"] != "":
            result["cover_image"] = download_cover_image(result["cover_image"])
        submit_huhu_draft(result)


# 下载封面图片
def download_cover_image(uri):
    main = urllib.parse.urlparse(uri).path
    filename = re.sub("/tos-cn-i-k3u1fbpfcp/", "", main)
    filename = re.sub(".image", "", filename)
    path = "./download/" + filename + ".jpg"
    r = requests.request('get', uri)
    if r.status_code == 200:
        with open(path, 'wb') as f:
            f.write(r.content)
        f.close()
        print(path + "下载成功！")
        return upload_huhu_dispose(filename, path)
    else:
        print("下载成功失败 error")


# 上传图片到乎乎服务器处理
def upload_huhu_dispose(filename, path):
    uri = "https://api.huhu.chat/image/get_img_url?spec=banzoom&wmark=false"
    file = {
        "File": (filename + ".jpg", open(path, "rb"), "image/jpg")
    }
    r = requests.post(uri, files=file)
    response = json.loads(r.text)
    if response["err_no"] == 0:
        return response["data"]["main_url"]


# 数据提交到乎乎草稿
def submit_huhu_draft(result):
    request = requests.post(
        url="https://api.huhu.chat/content_api/v1/article_draft/create",
        json={
            "title": result["title"],
            "brief_content": result["brief_content"],
            "mark_content": result["mark_content"],
            "cover_image": result["cover_image"],
            "category_id": "",
            "tag_ids": [],
            "edit_type": "md"
        },
        headers=token
    )
    response_json = json.loads(request.text)
    submit_huhu_article(response_json)


# 数据提交到乎乎文章
def submit_huhu_article(response_json):
    try:
        request = requests.post(
            url="https://api.huhu.chat/content_api/v1/article/publish",
            json={
                "draft_id": response_json["data"]["draft_id"],
                "title": response_json["data"]["title"],
                "brief_content": response_json["data"]["brief_content"],
                "mark_content": response_json["data"]["mark_content"],
                "cover_image": response_json["data"]["cover_image"],
                "category_id": "3964405574006734848",
                "tag_ids": [3975225018006110208],
                "edit_type": "md"
            },
            headers=token
        )
        response_json = json.loads(request.text)
        print(response_json["err_msg"])
        print("==========================================\n")
    except:
        print(response_json)
        print("==========================================\n")


if __name__ == '__main__':
    open_post()

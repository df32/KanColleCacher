KanColleCacher
====================
KanColleViewer插件，用于创建本地缓存以支持魔改并加快游戏加载速度



协议
--------------------
* 使用MIT开源协议发布。




使用方法
--------------------
* 将KanColleCacher.dll放入KanColleViewer的Plugin目录中。
* 将KanColleCacher.dll复制到KanColleViewer程序文件夹中的Plugins子文件夹中
* 启动KanColleViewer






设置缓存文件夹
--------------------
* 启动KanColleViewer并进入游戏模后
    * 点击「工具」页面的「缓存工具」标签页以显示缓存工具的设置页面
	* 在「缓存文件夹」标签后的文本框中输入文件夹绝对地址。
* 或者在启动KanColleViewer前
    * 在文件`%AppData%\grabacr.net\KanColleViewer\KanColleCacher.xml`中
	* 设置`<CacheFolder>`节点为缓存文件夹的绝对地址。

> **注意：**
> 
> 旧文件夹中的文件并不会被移动到新文件夹中；
> 
> 新的文件夹地址将在下次KanColleViewer启动时生效。






功能
--------------------

###文件缓存

        插件将在浏览器下载游戏文件后，将文件额外保存到缓存文件夹中。
        文件的保存位置为：`[缓存文件夹] \ [文件URL的路径]`
        例如：`E:\KanColleViewer\MyCache\kcs\scenes\TitleMain.swf`
        缓存功能可以通过设置「启用缓存」(`<CacheEnabled>`)来启用或禁用。



###指定缓存文件夹

        缓存文件夹的默认位置为KanColleViewer程序文件夹中的MyCache子文件夹。
        当文件夹不存在时自动创建文件夹。
        可以通过设置「缓存文件夹」(`<CacheFolder>`)来指定缓存文件夹。

> **注意：**
> 
> 文件夹地址应当设置绝对地址；
> 
> 旧文件夹中的文件并不会被移动到新文件夹中；
>
> 新的地址将在KanColleViewer下次启动时生效。



###文件分类与筛选
>

###Hack规则
>

###服务器图标与标题音效的规则
>

###文件版本校验
>





原理与机制
-------------------

###Fiddler
		Fiddler是KanColleViewer内嵌浏览器的代理器，KanColleViewer通过Fiddler收集游戏信息。
		KanColleCacher同样通过Fiddler来实现。


###会话
		每一次KanColleViewer向游戏服务器请求数据或下载文件都是一次会话。
		会话流程如下：

		客户端
		　↓　发送请求
		Fiddler
		　↓　发送请求
		服务器
		　↓　返回数据
		Fiddler
		　↓　返回数据
		客户端



###Fiddler规则
缓存工具共在三处添加了规则:

####BeforeRequest
客户端向Fiddler发送请求后，Fiddler向服务器发送请求前执行的规则：


####BeforeResponse
服务器向Fiddler返回数据后，Fiddler向客户端返回数据前：

####AfterSessionComplete
整个会话结束后


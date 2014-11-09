KanColleCacher
====================


简介
-----------------------

KanColleCacher.dll为网页游戏「舰队Collection」的工具程序「提督很忙！」（KanColleViewer，即KCV）的扩展插件。

KanColleCacher提供了在IE缓存外额外保存文件的功能（本地缓存）。
这一功能可以在一定程度上加快游戏加载速度，
并在此基础上支持对游戏文件的魔改。




功能介绍
-----------------------

###本地缓存

本地缓存是KanColleCacher提供的主要功能。

当IE缓存被清除后，客户端将重新从服务器下载游戏所需的文件。
KanColleCacher会在客户端向服务器请求文件之前，检查插件保存文件的“缓存文件夹”中是否存在相应的文件。

* 若文件存在且有效，则直接向客户端返回缓存文件夹中的文件，因此不需要再次下载文件。
* 若文件不存在，则客户端将下载文件，KanColleCacher将在文件下载完成后额外保存到缓存文件夹中。


###文件分类与筛选




版本与支持
-----------------------
* 目前KanColleCacher.dll为v1.2.0，
* 支持KanColleViewer v3.3\v3.4，
* 理论上不支持3.0以前的KCV，
* 不保证支持其他版本的KCV。


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

###本地缓存

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

KanColleViewer在设置中将游戏文件分为6类：

* 入口文件：在游戏加载前，可能与用户和初始化有关的文件，包括Core.swf, mainD2.swf。
* 载入文件：在出现GameStart按钮之前载入的文件，包括commonAsset.swf, font.swf, TitleMain.swf。
* 界面文件：在按下GameStart按钮后到母港出现前载入的文件，包括PortMain.swf, sound_se.swf。
* 场景文件
* 资源文件
* 声音文件




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


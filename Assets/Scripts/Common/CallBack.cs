public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate void CallBack<T, X>(T arg1, X arg2);
public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
public delegate void CallBack<T, X, Y, Z, H>(T arg1, X arg2, Y arg3, Z arg4, H arg5);

//c#自带的只支持到4个参数,此方法为自己实现，可以扩展很多参数
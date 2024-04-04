import {useSelector} from "react-redux";
import {RootState} from "../store/LoginSlice";
import {UserDataState} from "../store/UserDataSlice";
import {Navigate, useLocation} from "react-router-dom";

/* Для router с обязательной авторизацией и доступом по ролям*/
// @ts-ignore
function RequiredAuthAndOwnerOrAdminRole({children}){
    // @ts-ignore
    const isLogin = useSelector((state : RootState) => state.login.isLogin)
    // @ts-ignore
    const userRole = useSelector((state : UserDataState) => state.userData.roleName);
    const possibleRoles = ['Owner', 'Admin'];

    const location = useLocation();

    if(!isLogin){
        return <Navigate to={'/login'} state={{from: location}}/>;
    }

    if(!possibleRoles.includes(userRole)){
        return <Navigate to={'/'}/>;
    }

    return children;
}

export default RequiredAuthAndOwnerOrAdminRole;
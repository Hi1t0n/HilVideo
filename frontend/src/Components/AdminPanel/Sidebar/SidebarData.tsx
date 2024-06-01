import HomeIcon from '@mui/icons-material/Home';
import VideoCallIcon from '@mui/icons-material/VideoCall';
import VideocamOffIcon from '@mui/icons-material/VideocamOff';
import GroupAddIcon from '@mui/icons-material/GroupAdd';
import GroupRemoveIcon from '@mui/icons-material/GroupRemove';
import ExitToAppIcon from '@mui/icons-material/ExitToApp';

export const SidebarData = [
    {
        title: 'Главная',
        icon: <HomeIcon/>,
        link: '/adminpanel',
    },
    {
        title: 'Добавить фильм',
        icon: <VideoCallIcon/>,
        link: 'movie/add',
    },
    {
        title: 'Удалить фильм',
        icon: <VideocamOffIcon/>,
        link: 'movie/delete',
    },
    {
        title: 'Назначить админа',
        icon: <GroupAddIcon/>,
        link: 'admin/add',
    },
    {
        title: 'Снять админа',
        icon: <GroupRemoveIcon/>,
        link: 'admin/remove',
    },
    {
        title: 'Назад',
        icon: <ExitToAppIcon/>,
        link: '/',
    }
]

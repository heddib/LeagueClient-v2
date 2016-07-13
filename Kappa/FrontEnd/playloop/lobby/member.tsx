import { Swish }     from './../../ui/swish';
import Module        from './../../ui/module';

const template = (
    <module class="member">
        <div class="member" id="member-{{id}}">
            <div class="member-icon" style="background: url({{iconURL}})"></div>
            <span class="member-name">{{ name }}</span>
            <div class="member-roles">
                <div class="role1 role-{{role1}}"></div>
                <div class="role2 role-{{role2}}"></div>
            </div>
        </div>
    </module>
);
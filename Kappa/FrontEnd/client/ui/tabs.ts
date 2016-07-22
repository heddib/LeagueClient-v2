export function create(tabs: Swish[], start: number, handler: (old: number, now: number) => void) {
    let index = -1;
    let force = i => {
        if (i == index) return;

        if (tabs[index]) tabs[index].removeClass('active');
        tabs[i].addClass('active');

        handler(index, i);
        index = i;
    };
    for (let i = 0; i < tabs.length; i++) {
        tabs[i].on('click', () => force(i));
    }
    force(start);
    return force;
}
